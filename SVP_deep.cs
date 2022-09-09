using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using signalViewer;
using System.Threading;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Diagnostics;

namespace plugins_deepplayer
{
    /// <summary>
    /// This class describes the whole plugin. SignalPlant builds an instance of this class when a user needs to do it. 
    /// Technically, it is a user control docked in a frame (defined and created by a SignalPlant
    /// 
    /// Author: F. Plesinger, AIMT - ISI of the CAS, Brno, Czech Republic (2021-2022)
    /// 
    /// </summary>
    public partial class SVP_DEEP : UserControl
    {

        Color[] icols = {Color.SlateGray,Color.Red,Color.Orange,Color.YellowGreen,Color.DarkCyan,Color.Violet,Color.Pink,Color.Goldenrod,Color.DarkSeaGreen,Color.Cyan,Color.BlueViolet };

        List<signalViewer.graphPanel> linkedChannels = new List<signalViewer.graphPanel>(); //--array of linked channels

        string marksFilter = ""; //--filter, deffining marks selection
        List<selectionMember> marksList; //---array of linked marks


        ManualResetEvent COMRE = new ManualResetEvent(false); //---manual reset event. Important when this plugin generates a COMMAND too. Otherwise this can be deleted.

        float bgwPercentDone = 0;

        mainView mv; //-- instance of signal viewer program

        InferenceSession infs;//inference session for a model
        int numOutputChannels = 0; //number of model output channels. Will be loaded from a model
        int numInputChannels = 0;

        List<List<float>> outProbsPreview = null; //buffer for preview
        List<List<float>> outReal = null; //buffer for real output


        int winhs = 40; //height for displaying single iterating windows
        float prevPercDone = 0;

        string model_filename = "";
        private Exception prevError;
        private Exception procError;
        private Exception loadingError;

        List<float> previewTimes = null;

        string[] metadataOutputNames = null;
        float prefferedFS = float.NaN;
        private bool wantFreshRefresh;
        private readonly string noCudaCNTS= "No CUDA drivers found.";

        int specMess = -1;
        private int currentOutputHead=0;

        /// <summary>
        /// Default SP plugin method
        /// </summary>
        /// <returns>String with developer info</returns>
        public string getDeveloperInfo()
        {
            return "F. Plesinger/A. Ivora/P. Nejedly - Artificial Intelligence and Medical Technologies, Institute of Scientific Instruments of the CAS, Brno, Czech Republic, 2021-2022";
        }

        /// <summary>
        /// Default SP plugin method
        /// </summary>
        /// <returns>Returns plugin description </returns>
        public string getDescription()
        {

            return "Plugin to run deep-learning models inference";// +vi; //----enter description for your plugin. Plugin version info is already displayed by SignalPlant
        }

        /// <summary>
        /// Default SP plugin method. It is required to select submenu in SP/Mainmenu/Plugins is created during SP launch
        /// </summary>
        /// <returns>String with SP plugin category</returns>
        public string getCategory()
        {
            return "Generate data"; //---set category in plugins menu. If it does not exists, new category will be created
        }
                          

        /// <summary>
        /// Default SP plugin method. Controls the name of menu item in SP/Mainmenu/Plugins
        /// </summary>
        /// <returns>String plugin name</returns>
        public string getName()
        {
            return "Deep Player";        //---plugin name, visible in Plugins menu
        }

        /// <summary>
        /// This is called from signalViewer, when doing RefreshAllPluginForms()
        /// </summary>
        public void doExternalRefresh()
        {
            if (chbUpdateAuto.Checked)
            {
                refrControls();
                doPreviewWork();
            }

        }

        /// <summary>
        /// Seeks for a place where CUDA drivers are installed
        /// </summary>
        /// <returns>returns CUDA drivers location </returns>
        private string Get_CUDA_DriversDirectory()
        {
            string softwareKeyName = string.Empty;
            string homeDirectory = string.Empty;

            if (IntPtr.Size == 8)
            {
                softwareKeyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\GPU Computing Toolkit\CUDA";
            }
            else
            {
                softwareKeyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\NVIDIA Corporation\GPU Computing Toolkit\CUDA";
            }

            object regValue = Microsoft.Win32.Registry.GetValue(softwareKeyName, "FirstVersionInstalled", "No drivers found");
            if (regValue != null)
            {
                homeDirectory = regValue.ToString();
            }
            else
            {
                homeDirectory = noCudaCNTS;
            }
            return homeDirectory;
        }

        /// <summary>
        /// Default SP plugin method. Called when user select a preset in parent plugin form. 
        /// Used only when presets are used inside a plugin. They are not in the current DeepPlayer version.
        /// </summary>
        public void presetBeforeLoaded()
        {
            /*
          This funciotn is called after user clicks on preset in menu, but before corresponding controls are filled with new values. 
           */
        }

        /// <summary>
        /// Default SP plugin method. Called after preset was loaded. Not used in current DeepPlayer version.
        /// </summary>
        public void presetLoaded()
        {
            /*
             * This funciotn is called after user clicks on preset in menu. Corresponding controls (with filled TAG field) receives values from preset
             * and here you have to set values from controls to corresponding fields
            */

            refrControls();
        }


        /*
         //--When you need to have a command for scripts too, you have to implement these two methods:
         * 
         public string COMMAND_DEEPPLAYER(string parameters) //---change to COMMAND_SOMETHING if you need to name new command as "SOMETHING".
         {
             string pars = parameters.Substring(10);

             linkedChannels.Clear();

            

             string linkedChannelsFilter = "*";

             if (pars.IndexOf("CHANNEL(") > 0)
             {

                 int posAbbrev = pars.IndexOf(")");

                 int start = pars.IndexOf("CHANNEL(") + 8;

                 string sub1 = pars.Substring(start, posAbbrev-start+1);

                 linkedChannelsFilter = sub1.Substring(0, sub1.Length - 1);
             }

            

             mainView.footerMessage = "ShapeFinder started";


             bgw.RunWorkerAsync();

             COMRE.WaitOne();

             linkedChannels = null;
             GC.Collect();
             return "Done";
         }

         public static string CMDDESCRIPTION_NAME() //---change NAME to same as in the funcion before
         {
             return "FIND ANOTATIONS FOR CHALLENGE 2014";
         }

       */

        /// <summary>
        /// Called when SignalPlant needs to refresh DeepPlayer state (for example, during zoom in/out)
        /// </summary>
        private void refrControls()
        {


            btChannels.Text = "";
            if (linkedChannels.Count == 0) btChannels.Text = "Drag a channel here or click";
            if (linkedChannels.Count == 1) btChannels.Text = "Channel : " + linkedChannels[0].channelName;
            if (linkedChannels.Count > 1) btChannels.Text = linkedChannels.Count.ToString() + " channels are choosed";

            btProcess.Enabled = linkedChannels.Count > 0;

            if (bgw.IsBusy && bgw.CancellationPending) btProcess.Text = "Wait for cancellation";
            if (bgw.IsBusy && !bgw.CancellationPending) btProcess.Text = "Cancel";
            if (!bgw.IsBusy) btProcess.Text = "Process";


            pbx.Refresh();

        }

        /// <summary>
        /// Method to prepare a preview of model outputs
        /// </summary>
        private void doPreviewWork()
        {

            btPrev.PerformClick();

            pbx.Refresh();
        }

        /// <summary>
        /// Event when a user drags something (usually a channel from a SignalPlant) to a "Channels" button.
        /// </summary>
        /// <param name="sender">Who called this event</param>
        /// <param name="e">drag and drop parameters</param>
        private void btChannels_DragDrop(object sender, DragEventArgs e)
        {
            string s = e.Data.GetData(DataFormats.Text).ToString();

            try
            {
                linkedChannels.Add(mv.getGPbyName(s));
                refrControls();
                doPreviewWork();

            }
            catch (Exception exp)
            {
                mainView.log(exp, "Error while drag&drop", this); // this line will log error into "errorlog.txt"
                MessageBox.Show("Error:" + exp.Message);
            }
        }

        /// <summary>
        /// Standard control event. Occurs, when a user drags something and a mouse pointer enters target area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btChannels_DragEnter(object sender, DragEventArgs e)
        {
            string s = e.Data.GetData(DataFormats.Text).ToString();

            if (mv.getGPbyName(s) != null)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Method to computer standard deviation
        /// </summary>
        /// <param name="values">Input numerical array</param>
        /// <returns>Standard deviation</returns>
        public float getStd(float[] values)
        {
            float avg = values.Average();
            float sum = values.Sum(v => (v - avg) * (v - avg));
            float denominator = values.Length - 1;
            return denominator > 0.0 ? (float)(Math.Sqrt(sum / denominator)) : -1;
        }

        /// <summary>
        /// Control standard event when a user clicks the button - here, the channels select button. 
        /// It opens SignalPlant common dialog window to select from available channels (i.e. signals)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btChannels_Click(object sender, EventArgs e)
        {
            signalViewer.selectChannelForm sc = new signalViewer.selectChannelForm();
            sc.regenerateList(linkedChannels);

            if (sc.ShowDialog() == DialogResult.OK)
            {
                linkedChannels.Clear();
                for (int i = 0; i < sc.lv.SelectedItems.Count; i++)
                {
                    linkedChannels.Add(mv.getGPbyName(sc.lv.SelectedItems[i].Text));
                }
                refrControls();
                doPreviewWork();
            }
        }

        /// <summary>
        /// Happens when a user click on a Process button. If the process background worker is idle, than it starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btProcess_Click(object sender, EventArgs e)
        {
            if (!bgw.IsBusy)
            {
                bgw.RunWorkerAsync();
            }
            else
            {
                bgw.CancelAsync();
            }

            refrControls();
        }

        /// <summary>
        /// Class constructor. Sets default filter for marks (could be removed since Deep Player does not need them).
        /// Also, does initializes components and calls their first refresh.
        /// </summary>
        public SVP_DEEP()
        {
            InitializeComponent();

            signalViewer.selectMarkersForm sc = new signalViewer.selectMarkersForm();
            sc.setFilter(marksFilter);
            marksList = sc.result;

            refrControls();
        }

        /// <summary>
        /// Called from background worker to report progress in main picturebox (pbx) to inform the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            bgwPercentDone = e.ProgressPercentage;
            pbx.Refresh();
        }

        /// <summary>
        /// Method to draw dotted/dashed vertical line since .NET does not know how.
        /// </summary>
        /// <param name="gr">Graphic context to draw into</param>
        /// <param name="x">X location</param>
        /// <param name="y0">Y start</param>
        /// <param name="y1">Y end</param>
        /// <param name="spacing">Spacing between line segments</param>
        /// <param name="segheight">Height of segments</param>
        /// <param name="pn">Pen for the lined</param>
        private void drawDottedVertline(Graphics gr, int x,int y0,int y1, int spacing, int segheight, Pen pn)
        {

            for (int y = y0; y < y1; y += spacing + segheight)
                gr.DrawLine(pn, x, y, x,      y + segheight);

        }

        /// <summary>
        /// Main method to draw signal inputs/outpus and inference windows at the main picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbx_Paint(object sender, PaintEventArgs e)
        {
              //e.Graphics.DrawString("System:"+System.Environment.OSVersion.Platform.ToString()+ " | 64bit system:"+System.Environment.Is64BitOperatingSystem+" | 64bit process:"+System.Environment.Is64BitProcess, SystemFonts.DefaultFont, Brushes.Black, 4,pbx.Height-20);

            if (modelLoader.IsBusy)
            {
                e.Graphics.DrawString("Loading a model from "+model_filename, SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2, mainView.sfc);
                return;
            }

            if (linkedChannels == null || linkedChannels.Count == 0)
            {
                e.Graphics.DrawString("Please attach any channels first", SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2, mainView.sfc);
                return;
            }

            if (bgwPrev.IsBusy)
            {
                e.Graphics.DrawString("Preview in progress:"+prevPercDone+"%", SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2, mainView.sfc);
                int rw = (int)(pbx.Width * ((float)prevPercDone / 100));
                int rh = 4;
                e.Graphics.FillRectangle(Brushes.Black, 0, pbx.Height / 2 + 20, rw, rh);
                return;
            }

            if (bgw.IsBusy)
            {
                e.Graphics.DrawString("Computing in progress:" + bgwPercentDone + "%", SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2, mainView.sfc);
                int rw = (int)(pbx.Width * ((float)bgwPercentDone / 100));
                int rh = 4;
                e.Graphics.FillRectangle(Brushes.Black, 0, pbx.Height / 2 + 20, rw, rh);
                return;
            }


            if (prevError!=null)
            {
                if (linkedChannels.Count!=numInputChannels)
                {
                    e.Graphics.DrawString("An error occured during a preview: number of linked channels ("+linkedChannels.Count+") differs from a number of model inputs ("+numInputChannels+")" , SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2 - 30, mainView.sfc);
                    return;
                }

                e.Graphics.DrawString("An error occured during a preview:"+prevError.Message+"\r\nInner exception:"+prevError.InnerException , SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2-30, mainView.sfc);
                /*
                int rw = (int)(pbx.Width * ((float)prevPercDone / 100));
                int rh = 4;

                e.Graphics.FillRectangle(Brushes.Black, 0, pbx.Height / 2 + 20, rw, rh);
                */
                return;
            }

            if (procError != null)
            {
                string mss = "An error occured during a computation:" + procError.Message;

                if (!procError.Message.Contains("user"))
                    mss = mss +"\r\nInner exception:" + procError.InnerException;

                e.Graphics.DrawString(mss, SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2 - 30, mainView.sfc);
                return;
            }

            if (loadingError != null)
            {
                string et = "Model loading issue:\r\n" + loadingError.Message + "\r\nInner exception:" + loadingError.InnerException;

                if (specMess == 0)
                    et = et + "\r\n\r\nAnyway, this plugin needs \"Microsoft Visual C++ redistributable\". Have you installed it?";
                
                e.Graphics.DrawString(et, SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2 - 30, mainView.sfc);
                

                return;
            }

            e.Graphics.Clear(Color.White);


            if (infs == null)
            {
                e.Graphics.DrawString("Model not loaded", SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2-30, mainView.sfc);

            }

            if (previewTimes!=null && previewTimes.Count>0)
            {
                statistics stats = new statistics(null, previewTimes, 0, 0, "");

                string pstats = "Inference window size:" + nudWS.Value + ", count:" + previewTimes.Count + ". Mean inference time = " + previewTimes.Average().ToString("0.0") + " ± " + stats.dev.ToString("0.0") + " ms";
                string dev = rbCPU.Checked ? "CPU" : "GPU";

                pstats = pstats + " (" + dev + ")";

                pstats = pstats + "\r\nModel outputs:" + numOutputChannels;

                e.Graphics.DrawString(pstats, SystemFonts.DefaultFont, Brushes.Black, pbx.Width - 4, 4, mainView.sfr);
            }

            if (!Single.IsNaN(prefferedFS))
                if (prefferedFS != mainView.sampleFrequency)
            {
                //e.Graphics.DrawString("Signal FS != preffered model FS", SystemFonts.DefaultFont, Brushes.Black, 4, 4);
                lbPrefSamp.BackColor = Color.Red;
            }
            else
                lbPrefSamp.BackColor = Color.Transparent;

            try
            {

                graphPanel sch = linkedChannels[0];
                if (sch.dataCache==null)
                {
                    e.Graphics.Clear(Color.White);
                    e.Graphics.DrawString("Channel set was probably changed. Please, relink channels", SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2, mainView.sfc);
                    return;
                }
                dataCacheLevel dc = sch.dataCache[sch.currentDataChace];


                int ld = graphPanel.leftI;
                int rd = graphPanel.rightI;

                float[] sampleData = new float[rd - ld];

                float xScale = (float)pbx.Width / (float)(sampleData.Length);

                float lowestBase = 0;

                Font esf = new Font("Calibri", 8);

                float ybaseMain = 80;// pbx.Height / 2;
                if (rbUseFills.Checked)
                    ybaseMain = 50;

                //draw outputs:
                if (outProbsPreview != null && outProbsPreview.Count > 0)
                {
                    float defScale = 40;

                    if (rbUseFills.Checked)
                        defScale = 14;

                    float yscale = -defScale;

                    if (rbOutMax.Checked)
                    {
                        yscale = -(defScale / outProbsPreview.Count);
                        ybaseMain = 100;
                        if (rbUseFills.Checked)
                            ybaseMain = 60;

                    }

                    if (outProbsPreview.Count == 1)
                    {

                        float mx = outProbsPreview[0].Max();
                        float mn = outProbsPreview[0].Min();

                        float rng = mx;
                        if (rng == 0)
                            rng = 1;

                        yscale = defScale / rng;
                    }

                    int prevRight = 0;


                    int shownProbs = lbChannelUsage.SelectedItems.Count;
                    int sc = 0;

                    for (int c = 0; c < outProbsPreview.Count; c++)
                    {

                        if (!lbChannelUsage.GetSelected(c))
                            continue;

                        int step = 10;
                        if (rbUseFills.Checked)
                            step = 2;

                        float ybase = ybaseMain + sc * ((-yscale + step));

                        sc++;

                        if (chbDrawOver.Checked)
                            ybase = ybaseMain;


                        Pen pn = Pens.Black;
                        if (c < icols.Length)
                            pn = new Pen(icols[c]);

                        List<float> nums = outProbsPreview[c];

                        if (rbCurves.Checked)
                        {
                            PointF[] pts = new PointF[nums.Count];

                            bool anyNan = false;

                            for (int n = 0; n < nums.Count; n++)
                            {
                                pts[n] = new PointF(n * xScale, nums[n] * yscale + ybase);
                                if (Single.IsNaN(nums[n]))
                                    {
                                    anyNan = true;
                                    e.Graphics.DrawString("Probability array contains NAN value", SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2,mainView.sfc);
                                    return;

                                }
                            }

                            e.Graphics.DrawLines(pn, pts);
                        }


                        if (rbUseFills.Checked)
                        {
                            Color baseCol = pn.Color;


                            Brush brs = new SolidBrush(baseCol);

                            int lastRight = 0;
                            int lastRectPX = 0;
                            Rectangle rct = new Rectangle();
                            rct.Height = (int)defScale;
                            rct.Y = (int)(ybase + yscale);



                            for (int x = 1; x < nums.Count; x++)
                            {
                                if (nums[x] == nums[x - 1] && x < nums.Count - 1) continue;



                                int left = lastRight;
                                int wdth = x - left;

                                rct.X = (int)(left * xScale);
                                rct.Width = (int)(wdth * xScale);

                                if (rct.Width < 1)
                                    continue;




                                int alphaRat = convert01ValToAlpha(nums[x - 1]);
                                brs = new SolidBrush(Color.FromArgb(alphaRat, pn.Color));

                                if (rct.X > lastRectPX)
                                {
                                    //int diff = lastRectPX - rct.X;
                                    rct.X = lastRectPX;
                                    rct.Width = rct.Width + 1;// + diff;
                                }

                                if (chbDrawOver.Checked)
                                {
                                    if (nums[x - 1] > 0.5)
                                        e.Graphics.FillRectangle(brs, rct);
                                }
                                else
                                    e.Graphics.FillRectangle(brs, rct);

                                lastRectPX = rct.X + rct.Width;

                                /*                            
                                //cross-faded connection...just one rectangle
                                if (lastRight>0 && !chbDrawOver)
                                {
                                    float currVal = nums[x - 1];

                                    float avgVal = (currVal + lastRectVal) / 2;
                                    alphaRat = convert01ValToAlpha(avgVal);
                                    brs = new SolidBrush(Color.FromArgb(alphaRat, pn.Color));



                                    //e.Graphics.FillRectangle(brs, rct);

                                }
                            */


                                //lastRectVal = nums[x - 1];
                                lastRight = x;
                            }
                        }


                        //background 0-1 bar
                        if (!chbDrawOver.Checked && (chbDoSigm.Checked || chbSoftMax.Checked))
                        {

                            RectangleF rct = new RectangleF(0, ybase + yscale, pbx.Width, -yscale);

                            if (rbUseFills.Checked)
                            {
                                Pen pnL = new Pen(Color.FromArgb(40, pn.Color));

                                e.Graphics.DrawLine(pnL, rct.X, rct.Y, rct.Width + rct.X, rct.Y);
                                e.Graphics.DrawLine(pnL, rct.X, rct.Y + rct.Height, rct.Width + rct.X, rct.Y + rct.Height);
                            }
                            else
                                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(10, pn.Color)), rct);

                            if (rbCurves.Checked)
                            {
                                e.Graphics.DrawString("0", esf, Brushes.Black, 4, ybase - 14);
                                e.Graphics.DrawString("1", esf, Brushes.Black, 4, ybase + yscale + 2);
                            }
                        }



                        int xl = 10 + prevRight;
                        int xr = xl + 30;

                        string cnm = tbChannelBaseName.Text;

                        if (chbUM.Checked && c < metadataOutputNames.Length)
                            cnm = cnm + metadataOutputNames[c];

                        else
                            if (numOutputChannels>1)
                                cnm = cnm + c + "";

                        if (rbOutMax.Checked)
                        {
                            cnm = tbChannelBaseName.Text + "max";
                        }

                        SizeF ssize = e.Graphics.MeasureString(cnm, SystemFonts.DefaultFont);



                        int r = pn.Color.R * 5;
                        int g = pn.Color.G * 5;
                        int b = pn.Color.B * 5;

                        if (r < 200) r = 200;
                        if (g < 200) g = 200;
                        if (b < 200) b = 200;

                        if (r > 255) r = 255;
                        if (g > 255) g = 255;
                        if (b > 255) b = 255;

                        Brush brLight = new SolidBrush(Color.FromArgb(128, r, g, b));

                        //legend
                        if (chbDrawOver.Checked || rbUseFills.Checked)
                        {
                            int lineCenter = xl + (int)ssize.Width / 2;

                            e.Graphics.DrawLine(pn, lineCenter - 10, 5, lineCenter + 10, 5);
                            e.Graphics.DrawLine(pn, lineCenter - 10, 4, lineCenter + 10, 4);

                            //e.Graphics.FillRectangle(brLight, xl,12,ssize.Width,ssize.Height);
                            e.Graphics.DrawString(cnm, SystemFonts.DefaultFont, Brushes.Black, xl, 12);
                        }
                        else
                        {
                            //e.Graphics.FillRectangle(brLight, 10, ybase + yscale / 2 - 6,ssize.Width,ssize.Height);
                            e.Graphics.DrawString(cnm, SystemFonts.DefaultFont, Brushes.Black, 10, ybase + yscale / 2 - 6);
                        }

                        if (ybase > lowestBase)
                            lowestBase = ybase;


                        prevRight = xl + (int)ssize.Width;
                    }
                }

                lowestBase -= 10;

                int signalBoxHeight = (int)(pbx.Height - lowestBase - winhs);


                dc.data.CopyTo(ld, sampleData, 0, sampleData.Length);

                float resMinVal = sampleData.Min();
                float resMaxVal = sampleData.Max();

                float minVal = -5;
                float maxVal = 5;

                PointF prevT = mainView.computeScaleandBaseFromMinMax(resMinVal, resMaxVal, signalBoxHeight);
                PointF realT = mainView.computeScaleandBaseFromMinMax(minVal, maxVal, signalBoxHeight);

                if (rbFitOrig.Checked) prevT = realT;
                if (rbFitRes.Checked) realT = prevT;

                if (rbFitAll.Checked)
                {
                    float totMin = minVal < resMinVal ? minVal : resMinVal;
                    float totMax = maxVal > resMaxVal ? maxVal : resMaxVal;
                    prevT = mainView.computeScaleandBaseFromMinMax(totMin, totMax, signalBoxHeight);
                    realT = prevT;
                }

                float y1 = 0;
                float x1 = 0;

                PointF[] ptss = new PointF[sampleData.Length];

                for (int i = 0; i < sampleData.Length; i++)
                {
                    x1 = (float)i * xScale;
                    y1 = sampleData[i] * realT.X + realT.Y + lowestBase;
                    ptss[i] = new PointF(x1, y1);
                }

                e.Graphics.DrawLines(Pens.LightGray, ptss);

                //paint windows & cross-fades
                if (infs != null)
                {
                    int ws = (int)nudWS.Value;
                    int wo = (int)nudWO.Value;

                    int step = ws - wo;

                    int botW = pbx.Height - 1;
                    int topW = botW - winhs;

                    int wind = 1;

                    PointF[] pts = new PointF[4];

                    for (int x = 0; x * xScale < pbx.Width + step; x += step)
                    {
                        pts[0] = new PointF(x * xScale, botW);
                        pts[1] = new PointF((x + wo) * xScale, topW);
                        pts[2] = new PointF((x + step) * xScale, topW);
                        pts[3] = new PointF((x + step + wo) * xScale, botW);

                        Color clr = Color.DeepSkyBlue;

                        if (wind % 2 == 0)
                            clr = Color.Orange;

                        Brush tb = new SolidBrush(clr);

                        Pen pnw = new Pen(clr, 1);


                        e.Graphics.DrawLines(pnw, pts);

                        e.Graphics.DrawString("W " + wind, SystemFonts.DefaultFont, tb, (x + wo) * xScale, botW - 14);

                        wind++;

                        if (chbDrawWinBorders.Checked)
                        {
                            drawDottedVertline(e.Graphics, (int)pts[0].X, 38, (int)pts[0].Y, 10, 2, pnw);
                            drawDottedVertline(e.Graphics, (int)pts[3].X, 38, (int)pts[0].Y, 10, 2, pnw);
                        }

                    }


                }


            }catch (Exception ex)
            {
                e.Graphics.DrawString(ex.Message, SystemFonts.DefaultFont, Brushes.Black, pbx.Width / 2, pbx.Height / 2 - 30, mainView.sfc);
                return;
            }


        }

        /// <summary>
        /// Method mapping values in range 0-1 to a range 0-255
        /// </summary>
        /// <param name="v">probability 0-1</param>
        /// <returns>integer 0-255</returns>
        private int convert01ValToAlpha(float v)
        {
            float alpha = v;
            int alphaRat = (int)(alpha * 255);
            if (alphaRat < 0) alphaRat = 0;
            if (alphaRat > 255) alphaRat = 255;
            return alphaRat;
        }

        /// <summary>
        /// Method of the background worker who processes the whole signal when a user clicks the "Process button"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {

            //this is only slightly changed copy of the preview bgw.

            if (infs == null || linkedChannels.Count == 0)
                return;

            procError = null;

            //List<float> ptimes = new List<float>();

            Stopwatch sw = new Stopwatch();

            try
            {

                //prepare a tensor
                outReal = new List<List<float>>();

                graphPanel sch = linkedChannels[0];
                dataCacheLevel dc = sch.dataCache[sch.currentDataChace];

                int ld = 0;
                int rd = dc.data.Count;

                float[] sampleData = new float[rd - ld];
                dc.data.CopyTo(ld, sampleData, 0, sampleData.Length);


                //cut into windows
                int ws = (int)nudWS.Value; //window size
                int wo = (int)nudWO.Value; //window overlap

                int step = ws - wo;

                float[] signal = new float[ws];
                float[] signalPreproced = new float[ws];

                //int[] dimensions = infs.InputMetadata["input"].Dimensions;
                int[] dimensions = infs.InputMetadata.Values.ToList()[0].Dimensions;



                float percdone = 0;


                for (int x = 0; x < sampleData.Length + ws; x += step)
                {
                    if (bgw.CancellationPending)
                    {
                        procError = new Exception("Cancelled by a user");
                        return;
                    }


                    List<float> tensorData = new List<float>();

                    //iterate through associted channels

                    float avg = Single.NaN;
                    float std = Single.NaN;


                    foreach (graphPanel gp in linkedChannels)
                    {

                        dataCacheLevel dcl = gp.dataCache[gp.currentDataChace];
                        dcl.data.CopyTo(ld, sampleData, 0, sampleData.Length);

                        //in consecutive windows
                        if (x + ws < sampleData.Length)
                            Array.Copy(sampleData, x, signal, 0, signal.Length);
                        else
                        {
                            int remData = ws - (x + ws - sampleData.Length);
                            if (remData > 0)
                                Array.Copy(sampleData, x, signal, 0, remData);
                        }

                        if (rbPreprocNone.Checked)
                        {
                            signalPreproced = (float[])signal.Clone();
                        }

                        if (rbPreprocSTD.Checked)
                        {
                            //standardizace
                            std = getStd(signal);
                            avg = signal.Average();

                            if (std > 0)
                                for (int i = 0; i < signal.Length; i++)
                                    signalPreproced[i] = (signal[i] - avg) / std;
                            else
                                signalPreproced = (float[])signal.Clone();
                        }

                        if (rbPreprocNorm.Checked)
                        {

                            float mx = signal.Max();
                            float mn = signal.Min();

                            float sc = 1 / (mx - mn);


                            if (mx != mn)
                                for (int i = 0; i < signal.Length; i++)
                                    signalPreproced[i] = 2 * (1 - mn) * sc - 1;
                            else
                                signalPreproced = (float[])signal.Clone();
                        }


                        tensorData.AddRange(signalPreproced);

                    }




                    if (dimensions.Length == 4)
                    {
                        dimensions[3] = ws;
                        dimensions[2] = 1;
                    }
                    else
                        dimensions[2] = ws;

                    dimensions[0] = 1;

                    //dimensions[2] = ws;
                    //dimensions[0] = 1;

                    // and the dimensions of the input is stored here
                    //Tensor<float> ecgTensor = new DenseTensor<float>(signalPreproced, dimensions);
                    Tensor<float> ecgTensor = new DenseTensor<float>(tensorData.ToArray(), dimensions);

                    var inputTensor = new List<NamedOnnxValue>()
                  {
                  NamedOnnxValue.CreateFromTensor<float>("input", ecgTensor)
                  };


                    //sw.Restart();

                    List<float[]> outProbsLocal = new List<float[]>();

                    //Console.WriteLine("Starting inference");

                    sw.Restart();

                    using (var output = infs.Run(inputTensor))
                    {

                        sw.Stop();
                        //ptimes.Add(sw.ElapsedMilliseconds);

                        //Console.WriteLine("Inference done");

                        //var tns = output.First().AsTensor<float>();
                        var tns = output.ElementAt(currentOutputHead).AsTensor<float>();
                        float[] arr = tns.ToArray<float>();

                        if (chbDeSTD.Checked && numOutputChannels == 1)
                        {
                            for (int i = 0; i < arr.Length; i++)
                                arr[i] = arr[i] * std + avg;
                        }

                        int numSamples = tns.Dimensions[0]; //platí pro detekci patologií
                        if (tns.Dimensions.Length == 3)
                            numSamples = tns.Dimensions[2];

                        for (int c = 0; c < numOutputChannels; c++)
                        {
                            float[] locArr = new float[numSamples];

                            int startPos = c * locArr.Length;

                            //catch the case when we have only one sample(at multiple channels) for some window....:
                            if (numOutputChannels >= numSamples && numSamples == 1)
                            {
                                locArr = new float[ws];
                                for (int s = 0; s < ws; s++)
                                    locArr[s] = arr[c];
                            }
                            else
                                Array.Copy(arr, startPos, locArr, 0, locArr.Length);



                            //when DL model output is shorter then signal
                            if (numSamples > numOutputChannels && numSamples < signal.Length)
                            {
                                float rat = signal.Length / (float)numSamples;
                                int addSamples = (int)(rat + 1);

                                float[] backArr = (float[])locArr.Clone();

                                locArr = new float[ws];

                                for (int s = 0; s < backArr.Length; s++)
                                    for (int a = 0; a <= addSamples; a++)
                                    {
                                        int realPos = (int)(s * rat) + a;
                                        if (realPos < ws)
                                            locArr[realPos] = backArr[s];
                                    }
                            }

                            outProbsLocal.Add(locArr);
                        }




                        //Console.WriteLine("Data coppied");
                    }

                    //sw.Stop();
                    //times.Add(sw.ElapsedMilliseconds);

                    for (int o = 0; o < outProbsLocal.Count; o++)
                    {
                        if (outReal.Count < o + 1)
                            outReal.Add(new List<float>());


                        if (nudWO.Value == 0 || x == 0)
                            outReal[o].AddRange(outProbsLocal[o]);
                        else
                        {
                            //cross-fade last old "wo" samples with new "wo" samples

                            for (int xo = x; xo < x + wo; xo++)
                            {
                                int xn = xo - x;

                                float wb = (float)(xn) / wo;
                                float wa = 1 - wb;

                                float val = outReal[o][xo] * wa + outProbsLocal[o][xn] * wb;
                                outReal[o][xo] = val;
                            }

                            //add samples after the cross-fade
                            float[] afterCF = new float[ws - wo];

                            Array.Copy(outProbsLocal[o], wo, afterCF, 0, afterCF.Length);
                            outReal[o].AddRange(afterCF);

                        }

                    }

                    percdone = (int)((float)x / sampleData.Length * 100);
                    if (percdone > 100)
                        percdone = 100;

                    if ((int)percdone>0)
                        bgw.ReportProgress((int)percdone);

                }

                if (chbDoSigm.Checked && rbOutSeparated.Checked)
                    for (int o = 0; o < outReal.Count; o++)
                        for (int x = 0; x < outReal[o].Count; x++)
                        {
                            outReal[o][x] = (float)(1 / (1 + Math.Exp(-outReal[o][x])));
                        }


                if (chbSoftMax.Checked)
                {
                    float[] prbs = new float[outReal.Count];
                    float sumPrbs = 0;

                    for (int x = 0; x < outReal[0].Count; x++)
                    {
                        for (int o = 0; o < outReal.Count; o++)
                            prbs[o] = (float)Math.Exp(outReal[o][x]);

                        sumPrbs = prbs.Sum();

                        for (int o = 0; o < outReal.Count; o++)
                            outReal[o][x] = (float)(prbs[o] / sumPrbs);
                    }

                }

                if (rbOutMax.Checked)
                {
                    //only index of max channel will be reported

                    float[] repChan = new float[outReal[0].Count];
                    //float[] locField = new float[outReal.Count];

                    for (int x = 0; x < repChan.Length; x++)
                    {
                        int maxC = 0;
                        float maxV = Single.MinValue;

                        for (int c = 0; c < outReal.Count; c++)
                        {
                            float val = outReal[c][x];
                            if (val > maxV)
                            {
                                maxV = val;
                                maxC = c;
                            }
                        }

                        repChan[x] = maxC;
                    }
                    outReal = new List<List<float>>();

                    List<float> simple = new List<float>();
                    simple.AddRange(repChan);

                    outReal.Add(simple);
                }

                //converting output to channels

                for (int c=0; c<outReal.Count; c++)
                {
                    string cname = tbChannelBaseName.Text;

                    if (numOutputChannels > 1)
                        cname = cname + c + "";
                    

                    if (chbUM.Checked && c < metadataOutputNames.Length)
                        cname = tbChannelBaseName.Text + metadataOutputNames[c];

                    int idn = 0;

                    int addition = 0;

                    do
                    {

                        if (addition > 0)
                            cname = cname + "(" + addition + ")";

                        idn = mv.getChannelIndexByName(cname);

                        addition++;

                    }
                    while (idn >= 0);


                    float[] shortenData = new float[rd];

                    Array.Copy(outReal[c].ToArray(), 0, shortenData, 0, shortenData.Length);

                    graphPanel gp = new graphPanel(cname,shortenData.ToList(), mv);

                    gp.dataCache[0].name = "RAW";

                    if (!chbDeSTD.Checked)
                    {
                        gp.autoScale = false;
                        gp.manualYRange = true;
                        gp.manualYRangeMin = -0.1f;
                        gp.manualYRangeMax = 1.1f;
                    }

                    mv.addNewChannel(gp, 0, true);
                }

                mv.rebuiltAndRedrawAll();
                mv.safe_refresh_channel_list();




            }
            catch (Exception ex)
            {
                procError = ex;
            }



            COMRE.Set(); //---this is important only when implementing a command
        }


        /// <summary>
        /// Called when the background worker finishes processing linked signals in their whole length
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //refrControls();

            if (procError==null)
            {
                if (this.ParentForm != null)
                    this.ParentForm.Close();
            }
            else
            {
                btProcess.Text = "Compute";
                pbx.Refresh();
                    }




        }

        /// <summary>
        /// Event after a user click on button for marks selection. Not used in DeepPlayer now.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btMarks_Click(object sender, EventArgs e)
        {
            signalViewer.selectMarkersForm sc = new signalViewer.selectMarkersForm();
            sc.setFilter(marksFilter);

            if (sc.ShowDialog() == DialogResult.OK)
            {
                marksList = sc.result;
                marksFilter = sc.filter;
                refrControls();
            }
        }

        /// <summary>
        /// A method called during loading this plugin by SignalPlant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SVP_plugin_v3_Load(object sender, EventArgs e)
        {
            if (this.ParentForm != null)
            {
                mv = (mainView)Application.OpenForms[0]; //--this links mv property to signalplant application

                //test CUDA presence:
                string sc = Get_CUDA_DriversDirectory();

                if (string.IsNullOrEmpty(sc) || sc.Equals(noCudaCNTS))
                {
                    rbGPU.Enabled = false;
                    rbCPU.Checked = true;
                }
                else
                {
                    //MessageBox.Show(sc);
                    rbGPU.Text = "GPU (CUDA " + sc+" detected)";
                    rbGPU.Enabled = true;
                    rbGPU.Checked = true;
                }





            }
        }


        /// <summary>
        /// Event called when the user click on a button to load ONNX model into DeepPlayer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (ofd_onnx.ShowDialog() == DialogResult.OK)
            {
                string flnm = ofd_onnx.FileName;
                model_filename = flnm;
                loadONNX(flnm);
                
            }
        }

        /// <summary>
        /// Method to load the ONNX file
        /// </summary>
        /// <param name="flnm"></param>
        private void loadONNX(string flnm)
        {
            if (flnm.Length < 4)
                return;

            infs = null;
            
            if (!modelLoader.IsBusy)
                modelLoader.RunWorkerAsync();

            pbx.Refresh();
        }

        /// <summary>
        /// Method refreshing metadata from ONNX model
        /// </summary>
        private void RefreshMetadata()
        {
            if (infs == null)
            {
                lmi.Text = "---";
                lmo.Text = "---";
                btHeadLeft.Visible = false;
                btHeadRight.Visible = false;
                return;
            }

            try
            {

                prefferedFS = float.NaN;

                //int[] inputDims = infs.InputMetadata["input"].Dimensions;
                int[] inputDims = infs.InputMetadata.Values.ToList()[0].Dimensions;

                //int[] outputDims = infs.OutputMetadata["output"].Dimensions;
                int numOutpuHeads = infs.OutputMetadata.Count;

                if (numOutpuHeads>1)
                {
                    btHeadRight.Visible = true;
                    btHeadLeft.Visible = true;
                }

                int[] outputDims = infs.OutputMetadata.Values.ToList()[currentOutputHead].Dimensions;

                string producerName = infs.ModelMetadata.ProducerName;
                string descrition = infs.ModelMetadata.Description;
                string domain = infs.ModelMetadata.Domain;
                string graphname = infs.ModelMetadata.GraphName;
                string graphdesc = infs.ModelMetadata.GraphDescription;

                if (inputDims.Length == 1)
                {
                    lmi.Text = inputDims[0]+"";
                    MessageBox.Show("At least two output dimensions are required");
                }

                if (inputDims.Length == 2)
                {
                    lmi.Text = inputDims[0] + " x " + inputDims[1];
                    numInputChannels = inputDims[1];
                }

                if (inputDims.Length == 3)
                {
                    lmi.Text = inputDims[0] + " x " + inputDims[1] + " x " + inputDims[2];
                    numInputChannels = inputDims[1];
                }

                if (inputDims.Length == 4)
                {
                    lmi.Text = inputDims[0] + " x " + inputDims[1] + " x " + inputDims[2] + " x " + inputDims[3];
                    numInputChannels = inputDims[1];
                }



                if (outputDims.Length == 1)
                    lmo.Text = outputDims[0]+"";

                if (outputDims.Length == 2)
                    lmo.Text = outputDims[0] + " x " + outputDims[1];


                if (outputDims.Length == 3)
                    lmo.Text = outputDims[0] + " x " + outputDims[1] + " x " + outputDims[2];

                if (outputDims.Length == 4)
                    lmo.Text = outputDims[0] + " x " + outputDims[1] + " x " + outputDims[2]+ " x "+outputDims[3];

                if (numOutpuHeads > 1)
                    lmo.Text = lmo.Text+" (head " + (currentOutputHead+1) + "/" + numOutpuHeads + ")";

                lmdom.Text = domain;
                lmDesc.Text = descrition;
                lmprod.Text = producerName;

                lmgn.Text = graphname;
                lmgd.Text = graphdesc;

                prefferedFS = Single.NaN;
                lbPrefSamp.Text = "Preffered sample rate [Hz]:?";
                lbPrefSamp.BackColor = Color.Transparent;

                if (outputDims.Length>1)
                    numOutputChannels = outputDims[1];
                if (outputDims.Length == 1)
                    numOutputChannels = outputDims[0];

                gbBatch.Enabled = true;
                gbPreproc.Enabled = true;
                gbPostProc.Enabled = true;
                lbChannelUsage.Enabled = true;

                //labCross.Visible = nudWO.Value > 0;

                chbSoftMax.Enabled = numOutputChannels > 1;
                if (numOutputChannels == 1)
                    chbSoftMax.Checked = false;

                //loading metadata
                tbMTD.Text = "";
                string str="";
                var cmd = infs.ModelMetadata.CustomMetadataMap;

                metadataOutputNames = null;

                //probable samplecount
                int maxDimSize = inputDims.Max();
                if (maxDimSize > 1)
                    nudWS.Value = maxDimSize;

                foreach (string k in cmd.Keys)
                {
                    str = str + "Key: " + k + "\r\n";
                    str = str + cmd[k]+"\r\n";
                    str = str + "---------------------------------------------------------------------\r\n";

                    if (k=="output_names")
                    {
                        metadataOutputNames = cmd[k].Split(',');
                    }

                    if (k.ToLower().Equals("fs"))
                    {
                        prefferedFS = float.Parse(cmd[k]);
                        lbPrefSamp.Text = "Preffered sample rate [Hz]:"+prefferedFS;
                    }

                }

                chbUM.Enabled = metadataOutputNames != null;

                if (metadataOutputNames == null)
                    chbUM.Checked = false;
                else
                    chbUM.Checked = true;

                tbMTD.Text = str;

               

            }
            catch (Exception ex)
            {
                mainView.log(ex, "Reading metadata", this);
                //MessageBox.Show("Error loading a model:" + ex.Message);
            }
        }

        /// <summary>
        /// Method to load ONNX model from specific path. 
        /// Establishes new inference session object. 
        /// Also disables the graph optimization since it caused issues with some GRU/LSTM models
        /// </summary>
        /// <param name="pth">Path to ONNX file</param>
        private void LoadONNModel(string pth)
        {
            specMess = -1;

            try
            {
                loadingError = null;

                specMess = 0;
                SessionOptions se = new SessionOptions();
                specMess = 1;

                //se.AppendExecutionProvider_DML(0); //0,1,2 DirectML
                //se.AppendExecutionProvider_Tensorrt(0); //nepodařilo se rozjet

                //se.AppendExecutionProvider_CUDA(0); 

                //infs = new InferenceSession(pth, se);

                int gpuDeviceId = 0; // The GPU device ID to execute on

                specMess =2;

                if (rbGPU.Checked)
                {
                    SessionOptions sopt = SessionOptions.MakeSessionOptionWithCudaProvider(gpuDeviceId);
                    sopt.GraphOptimizationLevel = GraphOptimizationLevel.ORT_DISABLE_ALL;
                    infs = new InferenceSession(pth,sopt);
                }

                specMess = 3;

                if (rbCPU.Checked)
                    infs = new InferenceSession(pth, se);

                specMess = 4;
            }
            catch(Exception ex)
            {
                loadingError = ex;
                mainView.log(ex, "Model_loader", this);
                //MessageBox.Show("An error occured during model loading:" + ex.Message+"\r\n"+specMess+"\r\n"+System.Environment.OSVersion.Platform.ToString());
            }
            

            

        }


        /// <summary>
        /// Event called when a user chnages overlap settings for inference windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudOverLap_ValueChanged(object sender, EventArgs e)
        {
            //RefreshMetadata();
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// Event called when a user/SP wants a refresh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //pbx.Refresh();


            if (bgwPrev.IsBusy)
                wantFreshRefresh = true;

            if (!bgwPrev.IsBusy && !modelLoader.IsBusy)
                bgwPrev.RunWorkerAsync();
            


            //else
            //    bgwPrev.CancelAsync();
        }

        /// <summary>
        /// Method providing a preview 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgwPrev_DoWork(object sender, DoWorkEventArgs e)
        {
            if (infs == null || linkedChannels.Count==0)
                return;

            prevError = null;
            procError = null;

            List<float> ptimes = new List<float>();

            Stopwatch sw = new Stopwatch();

            try
            {

                //prepare a tensor
                outProbsPreview = new List<List<float>>();

                graphPanel sch = linkedChannels[0];
                dataCacheLevel dc = sch.dataCache[sch.currentDataChace];

                int ld = graphPanel.leftI;
                int rd = graphPanel.rightI;

                float[] sampleData = new float[rd - ld];
                dc.data.CopyTo(ld, sampleData, 0, sampleData.Length);


                //cut into windows
                int ws = (int)nudWS.Value; //window size
                int wo = (int)nudWO.Value; //window overlap

                int step = ws - wo;

                float[] signal = new float[ws];
                float[] signalPreproced = new float[ws];

                //int[] dimensions = infs.InputMetadata["input"].Dimensions;
                int[] dimensions = infs.InputMetadata.Values.ToList()[0].Dimensions;

                //List<List<float>> outProbsMulti = new List<List<float>>(); //správně definované pole pravděpodobností pro více výstupů

                float percdone = 0;


                for (int x = 0; x < sampleData.Length + ws; x += step)
                {
                    /*
                    if (bgwPrev.CancellationPending)
                    {
                        prevError = new Exception("Canceled by a user");
                        return;
                    }
                    */
                    List<float> tensorData = new List<float>();

                    //iterate through associted channels

                    float std = Single.NaN;
                    float avg = Single.NaN;

                    foreach (graphPanel gp in linkedChannels)
                    {

                        dataCacheLevel dcl = gp.dataCache[gp.currentDataChace];
                        dcl.data.CopyTo(ld, sampleData, 0, sampleData.Length);

                        //in consecutive windows
                        if (x + ws < sampleData.Length)
                            Array.Copy(sampleData, x, signal, 0, signal.Length);
                        else
                        {
                            int remData = ws - (x + ws - sampleData.Length);
                            if (remData > 0)
                                Array.Copy(sampleData, x, signal, 0, remData);
                        }

                        if (rbPreprocNone.Checked)
                        {
                            signalPreproced = (float[])signal.Clone();
                        }



                        if (rbPreprocSTD.Checked)
                        {
                            //standardizace
                            std = getStd(signal);
                            avg = signal.Average();

                            if (std > 0)
                                for (int i = 0; i < signal.Length; i++)
                                    signalPreproced[i] = (signal[i] - avg) / std;
                            else
                                signalPreproced = (float[])signal.Clone();
                        }

                        if (rbPreprocNorm.Checked)
                        {

                            float mx = signal.Max();
                            float mn = signal.Min();

                            float sc = 1 / (mx - mn);


                            if (mx != mn)
                                for (int i = 0; i < signal.Length; i++)
                                    signalPreproced[i] = 2 * (1 - mn) * sc - 1;
                            else
                                signalPreproced = (float[])signal.Clone();
                        }


                        tensorData.AddRange(signalPreproced);

                    }



                    if (dimensions.Length == 4)
                    {
                        dimensions[3] = ws;
                        dimensions[2] = 1;
                    }
                    else
                        dimensions[2] = ws;

                    dimensions[0] = 1;

                    // and the dimensions of the input is stored here
                    //Tensor<float> ecgTensor = new DenseTensor<float>(signalPreproced, dimensions);
                    Tensor<float> ecgTensor = new DenseTensor<float>(tensorData.ToArray(), dimensions);

                    var inputTensor = new List<NamedOnnxValue>()
                  {
                  NamedOnnxValue.CreateFromTensor<float>("input", ecgTensor)
                  };


                    //sw.Restart();

                    List<float[]> outProbsLocal = new List<float[]>();

                    //Console.WriteLine("Starting inference");

                    sw.Restart();

                    using (var output = infs.Run(inputTensor))
                    {

                        sw.Stop();
                        ptimes.Add(sw.ElapsedMilliseconds);

                        //Console.WriteLine("Inference done");
                        var tns = output.ElementAt(currentOutputHead).AsTensor<float>(); //intially First()

                        float[] arr = tns.ToArray<float>();


                        if (chbDeSTD.Checked && numOutputChannels==1)
                        {
                            for (int i = 0; i < arr.Length; i++)
                                arr[i] = arr[i] * std + avg;
                        }

                        int numSamples = tns.Dimensions[0]; //platí pro detekci patologií
                        if (tns.Dimensions.Length == 3)
                            numSamples = tns.Dimensions[2];

                        //TODO: if numSamples != input.length => rescalovat! Ale jak přesně? 3000 => 94
                        //here!
                        for (int c = 0; c < numOutputChannels; c++)
                        {
                            float[] locArr = new float[numSamples];

                            int startPos = c * locArr.Length;

                            //catch the case when we have only sample(at multiple channels) for some window....:
                            if (numOutputChannels >= numSamples && numSamples == 1)
                            {
                                locArr = new float[ws];
                                for (int s = 0; s < ws; s++)
                                    locArr[s] = arr[c];
                            }
                            else
                                Array.Copy(arr, startPos, locArr, 0, locArr.Length);

                            //when DL model output is shorter then signal
                            if (numSamples > numOutputChannels && numSamples < signal.Length)
                            {
                                float rat = signal.Length / (float)numSamples;
                                int addSamples = (int)(rat + 1);

                                float[] backArr = (float[])locArr.Clone();

                                locArr = new float[ws];
                                
                                for (int s = 0; s < backArr.Length; s++)
                                    for (int a=0;a<=addSamples;a++)
                                    {
                                        int realPos = (int)(s * rat)+a;
                                        if (realPos < ws)
                                            locArr[realPos] = backArr[s];
                                    }
                                    


                            }

                            outProbsLocal.Add(locArr);
                        }
                        //Console.WriteLine("Data coppied");
                    }

                    //sw.Stop();
                    //times.Add(sw.ElapsedMilliseconds);

                    for (int o = 0; o < outProbsLocal.Count; o++)
                    {
                        if (outProbsPreview.Count < o + 1)
                            outProbsPreview.Add(new List<float>());


                        if (nudWO.Value == 0 || x == 0)
                            outProbsPreview[o].AddRange(outProbsLocal[o]);
                        else
                        {
                            //cross-fade last old "wo" samples with new "wo" samples

                            for (int xo = x; xo < x + wo; xo++)
                            {
                                int xn = xo - x;

                                float wb = (float)(xn) / wo;
                                float wa = 1 - wb;

                                float val = outProbsPreview[o][xo] * wa + outProbsLocal[o][xn] * wb;
                                outProbsPreview[o][xo] = val;
                            }

                            //add samples after the cross-fade
                            float[] afterCF = new float[ws - wo];

                            Array.Copy(outProbsLocal[o], wo, afterCF, 0, afterCF.Length);
                            outProbsPreview[o].AddRange(afterCF);

                        }

                    }

                    percdone = (int)((float)x / sampleData.Length * 100);
                    if (percdone > 100)
                        percdone = 100;

                    bgwPrev.ReportProgress((int)percdone);

                }

                if (chbDoSigm.Checked && rbOutSeparated.Checked)
                    for (int o = 0; o < outProbsPreview.Count; o++)
                        for (int x = 0; x < outProbsPreview[o].Count; x++)
                        {
                            outProbsPreview[o][x] = (float)(1 / (1 + Math.Exp(-outProbsPreview[o][x])));
                        }

                if (chbSoftMax.Checked)
                {
                    double[] prbs = new double[outProbsPreview.Count];
                    double sumPrbs = 0;

                    for (int x = 0; x < outProbsPreview[0].Count; x++)
                    {


                        for (int o = 0; o < outProbsPreview.Count; o++)
                            prbs[o] = Math.Exp(outProbsPreview[o][x]);

                        sumPrbs = prbs.Sum();

                        for (int o = 0; o < outProbsPreview.Count; o++)
                            outProbsPreview[o][x] = (float)(prbs[o] / sumPrbs);
                    }

                }


                if (rbOutMax.Checked)
                {
                    //only index of max channel will be reported

                    float[] repChan = new float[outProbsPreview[0].Count];

                    //float[] locField = new float[outProbsPreview.Count];

                    for (int x = 0; x < repChan.Length; x++)
                    {
                        int maxC = 0;
                        float maxV = Single.MinValue;

                        for (int c = 0; c < outProbsPreview.Count; c++)
                        {
                            float val = outProbsPreview[c][x];
                            if (val > maxV)
                            {
                                maxV = val;
                                maxC = c;
                            }
                        }

                        repChan[x] = maxC;
                    }
                    outProbsPreview = new List<List<float>>();

                    List<float> simple = new List<float>();
                    simple.AddRange(repChan);

                    outProbsPreview.Add(simple);


                }

                previewTimes = new List<float>();

                previewTimes.AddRange(ptimes.ToArray());

            }
            catch(Exception ex)
            {
                prevError = ex;
            }

        }

        /// <summary>
        /// Event occurs when a user changes display settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbFitOrig_CheckedChanged(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Event occurs when a user changes display settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbFitRes_CheckedChanged(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Event occurs when a user changes display settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbFitAll_CheckedChanged(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Event occurs when a user changes display settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbFitBoth_CheckedChanged(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Event occurs when a user resize the plugin window => the plugin control is also resized since its docked inside.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SVP_DEEP_Resize(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Event occurs when a user changes inference window size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudWS_ValueChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked) 
                btPrev.PerformClick();
        }

        /// <summary>
        /// Event is called when the preview worker finish its work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgwPrev_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (wantFreshRefresh)
            {
                wantFreshRefresh = false;
                btPrev.PerformClick();
                    }
            else
            pbx.Refresh();
        }

        /// <summary>
        /// Radiobutton event when a user selects separate or single output from a model to export
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbOutSeparated_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// Event when a user changes output settings (only index of max. prob. output)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbOutMax_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// This is called when a user changes pre-processing to none
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPreprocNone_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// This is called when a user changes pre-processing to standardization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPreprocSTD_CheckedChanged(object sender, EventArgs e)
        {
            chbDeSTD.Enabled = rbPreprocSTD.Checked;

            if (!rbPreprocSTD.Checked)
                chbDeSTD.Checked = false;

            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// This is called when a user changes pre-processing to 0-1 normalization 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbPreprocNorm_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// This is called when a user want to apply sigmoid to the ouptput
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbDoSigm_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// This is called when a user changes output display to draw multiple outputs over themselfs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbDrawOver_CheckedChanged(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Event called when a preview worker needs to report a progress to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bgwPrev_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            prevPercDone = e.ProgressPercentage;
            pbx.Refresh();
        }

        /// <summary>
        /// Called when a user switches from GPU to CPU inference
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCPU_CheckedChanged(object sender, EventArgs e)
        {
            loadONNX(model_filename);
        }

        /// <summary>
        /// Called when a model needs to be loaded (async process inside also establishes a new inference session)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadONNModel(model_filename);
        }


        /// <summary>
        /// Occurs when a model is loaded and new inference session is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modelLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (loadingError == null)
            {
                lbf.Text = model_filename;
                RefreshMetadata();
                LBCrefresh();

                for (int i = 0; i < lbChannelUsage.Items.Count; i++)
                    lbChannelUsage.SetSelected(i, true);

                btPrev.PerformClick();
            }
            else
                pbx.Refresh();
        }


        /// <summary>
        /// Occurs when a user switch from CPU to GPU inference => new session with GPU support nust be created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbGPU_CheckedChanged(object sender, EventArgs e)
        {
            loadONNX(model_filename);
        }

        /// <summary>
        /// Occurs when a user needs to draw inference windows borders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbDrawWinBorders_CheckedChanged(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Occurs when a user switch using a metada for model output channels
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbUM_CheckedChanged(object sender, EventArgs e)
        {
            LBCrefresh();
            pbx.Refresh();
        }

        /// <summary>
        /// Occurs when a user switch a SoftMax for preprocessing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbSoftMax_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }


        /// <summary>
        /// Occurs when a user changes display mode for outputs:curves or fills
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCurves_CheckedChanged(object sender, EventArgs e)
        {
            pbx.Refresh();
        }

        /// <summary>
        /// Occurs when a user release a key while typing inside a textbox to define prescriptor for model outputs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbChannelBaseName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LBCrefresh();
                pbx.Refresh();
            }
        }

        /// <summary>
        /// Refreshes names in the visible output selector
        /// </summary>
        private void LBCrefresh()
        {
            

            lbChannelUsage.Items.Clear();

            for (int c=0; c<numOutputChannels;c++)
            {
                string cnm = tbChannelBaseName.Text;

                if (chbUM.Checked && c < metadataOutputNames.Length)
                    cnm = cnm + metadataOutputNames[c];

                else
                    if (numOutputChannels > 1)
                    cnm = cnm + c + "";

                if (rbOutMax.Checked)
                {
                    cnm = tbChannelBaseName.Text + "max";
                }

                lbChannelUsage.Items.Add(cnm);
            }
        }

        /// <summary>
        /// Occurs when a user changes a use of de-standardization in regression tasks 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chbDeSTD_CheckedChanged(object sender, EventArgs e)
        {
            if (chbUpdateAuto.Checked)
                btPrev.PerformClick();
        }

        /// <summary>
        /// Occurs when a user change selection of output channels to display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbChannelUsage_SelectedValueChanged(object sender, EventArgs e)
        {
            refreshSelectedCount();
        }

        /// <summary>
        /// Refreshes information about selected outputs
        /// </summary>
        private void refreshSelectedCount()
        {
            int cnt = lbChannelUsage.SelectedItems.Count;

            if (cnt==0 && lbChannelUsage.Items.Count>0)
            {
                MessageBox.Show("At least one output must be selected. Selecting the first now...");
                lbChannelUsage.SetSelected(0, true);
                lbSO.Text = "Selected outputs: 1/" + numOutputChannels;
            }
            else
            lbSO.Text = "Selected outputs: " + cnt + "/" + numOutputChannels;
            pbx.Refresh();
        }

        /// <summary>
        /// Occurs when user moves to lower output head (to the left)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHeadLeft_Click(object sender, EventArgs e)
        {
            int oldCOH = currentOutputHead;
            currentOutputHead--;
            if (currentOutputHead < 0)
                currentOutputHead = 0;

            if (oldCOH != currentOutputHead)
            {
                RefreshMetadata();
                btPrev.PerformClick();
            }
        }

        /// <summary>
        /// Occurs when a user moves to the higher output head (to the right)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHeadRight_Click(object sender, EventArgs e)
        {
            if (infs == null)
                return;

            int oldCOH = currentOutputHead;
            currentOutputHead++;
            if (currentOutputHead >= infs.OutputMetadata.Count)
                currentOutputHead = infs.OutputMetadata.Count - 1;

            if (oldCOH != currentOutputHead)
            {
                RefreshMetadata();
                btPrev.PerformClick();
            }
        }
    }
}
