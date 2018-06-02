using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UserLedTest
{
    public partial class LedTest : UserControl
    {
        //显示的数字的接口
        public int num
        {
            set
            {
                if (value < 0 || value >= 100000)
                    number = 00000;
                else
                    number = value;
                Invalidate();
            }
        }
        
        //用户保存原始控件的高度和宽度
        struct oldLedCtrlSize
        {
            public double oldWidth;
            public double oldHeight;
            public double oldLedCtrWidth;
            public double oldLedCtrHeight;
        }

        //控件大小改变后的高度和宽度
        struct newLedCtrlSize
        {
            public double nWidth;
            public double nHeight;
        }

            struct LedCtrlPoint
            {
                public double width;
                public double height;

                public double oldLocatX;
            }

            LedCtrl[] ledCrl = new LedCtrl[5];
            oldLedCtrlSize oldLedSize = new oldLedCtrlSize();
            LedCtrlPoint ledCtrlPoint = new LedCtrlPoint();
            private int number;

        public LedTest()
        {
            InitializeComponent();
            oldLedSize.oldHeight = this.Height;
            oldLedSize.oldWidth = this.Width;
            oldLedSize.oldLedCtrWidth = ledCtr1.Width;
            oldLedSize.oldLedCtrHeight = ledCtr1.Height;
                //oldLocatY = ledCtr2.Location.X - ledCtr1.Location.X;
            ledCrl[0] = ledCtr1;
            ledCrl[1] = ledCtr2;
            ledCrl[2] = ledCtr3;
            ledCrl[3] = ledCtr4;
            ledCrl[4] = ledCtr5;
            ledCtrlPoint.oldLocatX = ledCtr2.Location.X - ledCtr1.Location.X;
        }

        //显示数字
        void ShowNum()
        {
            int[] num = new int[5];
            num[4] = number % 10;
            num[3] = number / 10 % 10;
            num[2] = number / 100 % 10;
            num[1] = number / 1000 % 10;
            num[0] = number / 10000;

            for (int i = 0; i < 5; i++)
            {
                ledCrl[i].DisplayNumber = num[i];
            }
        }

        private void LedTest_Paint(object sender, PaintEventArgs e)
        {    
            ShowNum();
        }

        private void LedTest_Load(object sender, EventArgs e)
        {
            ledCtrlPoint.width = ledCtr1.Width;
            ledCtrlPoint.height = ledCtr1.Height;
          
        }

        private void LedTest_Resize(object sender, EventArgs e)
        {
            newLedCtrlSize newLedCtlSize = new newLedCtrlSize();
            newLedCtlSize.nHeight = this.Height;//获取控件改变后控件的高
            newLedCtlSize.nWidth = this.Width;//获取控件改变后控件的宽

            double newHeight = newLedCtlSize.nHeight / oldLedSize.oldHeight;
            double newWidth = newLedCtlSize.nWidth / oldLedSize.oldWidth;

            int heightSize = (int)(newHeight * oldLedSize.oldLedCtrHeight);
            int widthSize = (int)(newWidth * oldLedSize.oldLedCtrWidth);

            double newLocatX = newWidth * ledCtrlPoint.oldLocatX;

            int LocatX = ledCtr1.Location.X;

            for (int i = 0; i < 5; i++)
            {
                ledCrl[i].Location = new System.Drawing.Point(LocatX, -1);//控件显示的位置
                ledCrl[i].Size = new System.Drawing.Size(widthSize, heightSize);//控件的大小
                ledCrl[i].SectionThick = (int)(6 * newHeight);//设置字体画笔的宽度
                LocatX += (int)newLocatX;
                //MessageBox.Show(LocatX.ToString());
            }
        }

        private void LedTest_SizeChanged(object sender, EventArgs e)
        {
            newLedCtrlSize newLedCtlSize = new newLedCtrlSize();
            newLedCtlSize.nHeight = this.Height;
            newLedCtlSize.nWidth = this.Width;

            double newHeight = newLedCtlSize.nHeight / oldLedSize.oldHeight;
            double newWidth = newLedCtlSize.nWidth / oldLedSize.oldWidth;

            int heightSize = (int)(newHeight * oldLedSize.oldLedCtrHeight);
            int widthSize = (int)(newWidth * oldLedSize.oldLedCtrWidth);

            double newLocatX = newWidth * ledCtrlPoint.oldLocatX;
            
            int LocatX = ledCtr1.Location.X;

            //MessageBox.Show(ledCtrlPoint.oldLocatX.ToString());
            //MessageBox.Show(widthSize.ToString());

            for (int i = 0; i < 5; i++)
            {
                ledCrl[i].Location = new System.Drawing.Point(LocatX, -1);
                ledCrl[i].Size = new System.Drawing.Size(widthSize, heightSize);
                ledCrl[i].SectionThick = (int)(6 * newHeight);
                LocatX += (int)newLocatX;
                //MessageBox.Show(LocatX.ToString());
            }
        }
    }
}
