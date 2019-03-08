using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Video.FFMPEG;
using AForge.Video;
using System.Diagnostics;

using CSCore;
using CSCore.SoundIn;
using CSCore.Codecs.WAV;

namespace ScreenRecord
{
    public partial class Form1 : Form
    {
        private bool _isRecording;
        private List<string> _screenNames;
        private Rectangle _screenSize;
        private UInt32 _frameCount;
        private int fps;
        private VideoFileWriter _writer;
        private int _width;
        private int _height;
        private ScreenCaptureStream _streamVideo;
        private Stopwatch _stopWatch;

        private Stopwatch _rebootWatch;

        int leftTime,totalTime;
        int elapse;

        private Rectangle _screenArea;

        
        private WasapiCapture capture = null;
        private WaveWriter w = null;

        static string fullName;


        Form2 form2 = new Form2();

        static EventWaitHandle _waitHandle = new AutoResetEvent(false);



        private void StartRec(string path)
        {
            form2.Opacity = .1;

            //form2.Hide();

            if (_isRecording == false)
            {

                this.SetScreenArea();
               
                this.SetVisible(true);

                this._frameCount = 0;
                this.fps = 10;
                this.tb_SaveFolder.Text = path;
                fullName = string.Format(@"{0}\{1}", path, DateTime.Now.ToString("MM_dd_HH_mm_ss"));
                if (checkBox2.Checked) {
                    fullName += "_News";
                }
                //string fullName2 = string.Format(@"{0}\{1}_{2}.wav", path, Environment.UserName.ToUpper(), DateTime.Now.ToString("d_MMM_yyyy_HH_mm_ssff"));
                // Save File option
                _writer.Open(
                    fullName + ".avi",
                    this._width,
                    this._height,
                    fps,
                    (VideoCodec)cb_VideoCodec.SelectedValue,
                    (int)(BitRate)this.cb_BitRate.SelectedValue);

                // Start main work
                this.StartRecord();
                this._isRecording = true;
                if (checkBox1.Checked == true)
                {
                    //start audio
                    
                    //wri = new LameMP3FileWriter(@"C:\Temp\test_output.mp3", waveIn.WaveFormat, 32);
                    // wri = new LameMP3FileWriter(fullName + ".wav", waveIn.WaveFormat, 32);
                    
                    capture = new CSCore.SoundIn.WasapiLoopbackCapture();
                    capture.Initialize();
                    w = new WaveWriter(fullName + ".wav", capture.WaveFormat);
                    //setup an eventhandler to receive the recorded data
                    capture.DataAvailable += (s, capData) =>
                    {
                        //save the recorded audio
                        w.Write(capData.Data, capData.Offset, capData.ByteCount);
                    };
                    //start recording
                    capture.Start();
                }

            }
        }

        public Form1()
        {
         
            form2.Show();
            InitializeComponent();

           
            int x = (int)Screen.PrimaryScreen.Bounds.Width;
            int y = (int)Screen.PrimaryScreen.Bounds.Height;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(x-this.Width, y-this.Height);
            toolStripStatusLabel1.Text = "Enter F11 or Press Start Button to stat recording";
            // string fullName; 

            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            this._isRecording = false;
            this._screenSize = Screen.PrimaryScreen.Bounds;
            this._frameCount = 0;
            this._width = SystemInformation.VirtualScreen.Width;
            this._height = SystemInformation.VirtualScreen.Height;


            this._stopWatch = new Stopwatch();
            this._rebootWatch = new Stopwatch();

            this._screenArea = Rectangle.Empty;

            this.bt_Save.Enabled = false;
            this._writer = new VideoFileWriter();

            _screenNames = new List<string>();
            _screenNames.Add( @"Select ALL" );
            foreach ( var screen in Screen.AllScreens )
            {
                _screenNames.Add(screen.DeviceName);
            }
            this.cb_screenSelector.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cb_screenSelector.DataSource = _screenNames;

            // Codec ComboBox
            this.cb_VideoCodec.DataSource = Enum.GetValues( typeof( VideoCodec ) );
            this.cb_VideoCodec.DropDownStyle = ComboBoxStyle.DropDownList;

            // BitRate 2000kbit/s 2000000 1000000
            this.cb_BitRate.DataSource = Enum.GetValues( typeof( BitRate ) );
            this.cb_BitRate.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cb_BitRate.SelectedIndex = 5;  //비트레이트 초기값

            this.tb_SaveFolder.Text = "F:\\Recorder";

            // 자동종료 초기값
            var list = new List<Level>
                        {
                            new Level {Name = "30분 후", Number = 30},
                            new Level {Name = "1시간 후", Number = 60},
                            new Level {Name = "1시간30분 후", Number = 90},
                            new Level {Name = "2시간 후", Number = 120}
                        };

            comboBox1.ValueMember = "Number";
            comboBox1.DisplayMember = "Name";
            comboBox1.DataSource = list;

           

        }

        public void bt_start_Click( object sender, EventArgs e )
        {
            this.WindowState = FormWindowState.Minimized;
           
            try
            {
                if ( this.tb_SaveFolder.Text.Length < 1 )
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    if ( fbd.ShowDialog() == DialogResult.OK )
                    {
                        this.StartRec( fbd.SelectedPath );
                    }
                }
                else
                {
                    if (Directory.Exists(this.tb_SaveFolder.Text))
                    {
                        this.StartRec(this.tb_SaveFolder.Text);
                    } else
                    {
                        MessageBox.Show("The specified folder was not found.");
                    }
                }
            }
            catch ( Exception exc )
            {
                MessageBox.Show( exc.Message );
            }
            
        }

      

        private void SetScreenArea()
        {
            // get entire desktop area size
            string screenName = this.cb_screenSelector.SelectedValue.ToString();
            if ( string.Compare( screenName, @"Select ALL", StringComparison.OrdinalIgnoreCase) == 0 )
            {
                foreach ( Screen screen in Screen.AllScreens )
                {
                    this._screenArea = new Rectangle(0,0,0,0);
                    //this._screenArea = Rectangle.Union( _screenArea, screen.Bounds );
                 
                    if (form2.Width % 2 == 1) form2.Width = form2.Width + 1;
                    this._screenArea.Width = form2.Width;
                    this._width = form2.Width;
                    if (form2.Height % 2 == 1) form2.Height = form2.Height + 1;
                    this._screenArea.Height = form2.Height;
                    this._height = form2.Height;

                    this._screenArea.Offset(form2.Location.X, form2.Location.Y);

                }
            }
            else
            {
                this._screenArea = Screen.AllScreens.First( scr => scr.DeviceName.Equals( screenName ) ).Bounds;
                
            }
        }

        private void StartRecord() //Object stateInfo
        {
           
            // create screen capture video source
            this._streamVideo = new ScreenCaptureStream( this._screenArea );
            //MessageBox.Show(form2.Location.X.ToString() + " " + form2.Location.Y.ToString());
            //MessageBox.Show(this._screenArea.Width.ToString() + " " + this._screenArea.Height.ToString());
            // set NewFrame event handler
            this._streamVideo.NewFrame += new NewFrameEventHandler( this.video_NewFrame );

            // start the video source
            this._streamVideo.Start();

            // _stopWatch
            this._stopWatch.Start();
            if (checkBox3.Checked)
            {
                // MessageBox.Show("automatic termination is initiated");

                // _stopWatch
                this._rebootWatch.Start();

                

                int selectedIndex = comboBox1.SelectedIndex;
                int selectedValue = (int)comboBox1.SelectedValue;

                totalTime = selectedValue*60;

                leftTime = totalTime;

                

            }
        }

        private void video_NewFrame( object sender, NewFrameEventArgs eventArgs )
        {
            if ( this._isRecording )
            {
                this._frameCount += Convert.ToUInt32(fps);
                this._writer.WriteVideoFrame( eventArgs.Frame );
               
                this.lb_stopWatch.Invoke(new Action(() =>
              {
              this.lb_stopWatch.Text = string.Format("{0:00}:{1:00}:{2:00}", _stopWatch.Elapsed.Hours, _stopWatch.Elapsed.Minutes, (int)_stopWatch.Elapsed.Seconds);
                  
              }) );
                                                            

                this.lb_rebootWatch.Invoke(new Action(() =>
                {
                    if (checkBox3.Checked)
                    {

                        leftTime = totalTime - (int)_stopWatch.Elapsed.TotalSeconds;
                        TimeSpan t = TimeSpan.FromSeconds(leftTime);
                        //this.lb_rebootWatch.Text = string.Format("{0:00}:{1:00}:{2:00}", h_leftTime, m_leftTime, s_leftTime);
                        this.lb_rebootWatch.Text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                                                                        t.Hours,
                                                                        t.Minutes,
                                                                        t.Seconds);

                        if (leftTime == 0)
                        {
                            // terminate system
                            bt_Save_Click(sender, eventArgs);

                          

                        }
                    }
                }));
            }
            else
            {
                _stopWatch.Reset(); // Terninate Thread
                _rebootWatch.Reset(); // Terninate Thread

                _streamVideo.SignalToStop();
                
                _writer.Close(); // End Recoding

                //stop recording
                capture.Stop();
                w.Dispose();
                w = null;
                capture.Dispose();
                capture = null;

                if (checkBox1.Checked == true)
                {
                    string files = "-i " + fullName + ".avi -i " + fullName + ".wav -c:v copy -c:a aac -strict experimental " + fullName + ".mp4";
                    string cmdtext = "/C ffmpeg " + files;
                    var process = System.Diagnostics.Process.Start("CMD.exe", cmdtext);
                    process.WaitForExit();

                    try
                    {
                        System.IO.File.Delete(fullName + ".avi");
                        System.IO.File.Delete(fullName + ".wav");
                    }
                    catch { MessageBox.Show("fail"); }

                }

                if (checkBox3.Checked == true && leftTime == 0)
                {
                    System.Windows.Forms.Application.Exit();
                    Process.Start("shutdown", "/s /t 0");

                }


            }
        }

      

        private void bt_Save_Click( object sender, EventArgs e )
        {
            this.WindowState = FormWindowState.Normal;

            this.SetVisible( false );

            this._isRecording = false;

          
           
            form2.TopMost = true;
            form2.Opacity = .6;
            form2.Show();

            //MessageBox.Show(@"File saved!");


            


        }

        private void SetVisible( bool visible )
        {
            this.bt_start.Enabled = !visible;
            this.bt_Save.Enabled = visible;
            this._isRecording = visible;
        }

        private void Form1_FormClosing( object sender, FormClosingEventArgs e )
        {
            this._isRecording = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.F11:
                    //MessageBox.Show("F1 pressed.", "Key Down Event");
                    bt_start_Click(sender, e);
                    this.WindowState = FormWindowState.Minimized;
                    break;
                case Keys.Escape:
                    //MessageBox.Show("F1 pressed.", "Key Down Event");
                    bt_Save_Click(sender, e);
                    //this.WindowState = FormWindowState.Minimized;
                    break;
                case Keys.Space:
                    //MessageBox.Show("F1 pressed.", "Key Down Event");
                    if (_isRecording)
                    {

                    }
                    else {

                    }

                    //this.WindowState = FormWindowState.Minimized;
                    break;
                default:
                    //MessageBox.Show(e.KeyCode.ToString() + " pressed.", "Key Down Event");
                    break;
            }
            
           
        }

        private void cb_BitRate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public enum BitRate
    {
        _3000kbit = 3000000,
        _4000kbit = 4000000,
        _5000kbit = 5000000,
        _6000kbit = 6000000,
        _7000kbit = 7000000,
        _8000kbit = 8000000,

    }
    public class Level
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }

    // Detect all numeric characters at the form level and consume 1, 
    // 4, and 7. Note that Form.KeyPreview must be set to true for this
    // event handler to be called.
   

}
