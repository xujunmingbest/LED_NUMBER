using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserLedTest
{
    public struct Section
    {
        public Section(int numberofPoints)
        {
            this.P = new Point[numberofPoints];
        }
        /// <summary>
        /// 点数组
        /// </summary>
        public Point[] P;
    }

    public partial class LedCtrl : UserControl
    {
        private byte m_DisplayCode = (byte)0x37;

        /// <summary>
        /// 段熄灭时的颜色，点亮为前景色
        /// </summary>
        private Color m_OffColor = Color.FromArgb(20, 50, 50);

        /// <summary>
        /// 用offcolor绘制熄灭笔画的边缘，而不是填充它！
        /// </summary>
        private bool b_DrawSectionBorder = true;
        /// <summary>
        /// 画刷
        /// </summary>
        private SolidBrush m_Brush;
        /// <summary>
        /// 铅笔
        /// </summary>
        private Pen m_Pen;
        /// <summary>
        /// 笔画宽度
        /// </summary>
        private int m_SectionThick = 4;
        /// <summary>
        /// 段0~6点数组！！！注意，这7段并不包括小数点！！！
        /// </summary>
        private Section[] m_Sections = new Section[7];
        /// <summary>
        /// 右下角的小数点
        /// </summary>
        private Point m_Dot = new Point(0, 0);

        /// <summary>
        /// 一些基本的字符编码
        /// </summary>
        private byte[] m_NumCodes =
			{
				(byte)0x7d,//0
				(byte)0x50,//1
				(byte)0x37,//2
				(byte)0x57,//3
				(byte)0x5a,//4
				(byte)0x4f,//5
				(byte)0x6f,//6
				(byte)0x54,//7
				(byte)0x7f,//8
				(byte)0x5f,//9
				(byte)0x00,//turn off all sections
				(byte)0x02,//-
			};
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;

        public LedCtrl()
        {
            InitializeComponent();
            this.SetStyle(
                ControlStyles.DoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw,
                true);
            this.UpdateStyles();
            this.m_Pen = new Pen(this.m_OffColor);
            this.m_Brush = new SolidBrush(this.BackColor);
            for (int i = 0; i < this.m_Sections.Length; i++)
            {
                this.m_Sections[i] = new Section(6);
            }
            //计算段坐标
            this.ComputeSections(this.Width, this.Height);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool DrawSectionBorder
        {
            get { return this.b_DrawSectionBorder; }
            set
            {
                if (this.b_DrawSectionBorder != value)
                {
                    this.b_DrawSectionBorder = value;
                    this.Invalidate();
                }
            }
        }

        public byte DisplayCode
        {
            get
            {
                return this.m_DisplayCode;
            }
            set
            {
                if (this.m_DisplayCode != value)
                {
                    this.m_DisplayCode = value;
                    this.Invalidate();
                }
            }
        }

        public int DisplayNumber
        {
            get
            {
                //如果返回-1说明显示的不是数字！！！
                return Array.IndexOf(this.m_NumCodes, (byte)this.m_DisplayCode);
            }
            set
            {
                if (value < 0 || value > 9)
                {
                    //如果是超出了0~9的范围，则全熄灭
                    this.DisplayCode = (byte)0x00;
                    return;
                }
                this.DisplayCode = this.m_NumCodes[value];
            }
        }


        public int SectionThick
        {
            get
            {
                return this.m_SectionThick;
            }
            set
            {
                if (this.m_SectionThick != value)
                {
                    this.m_SectionThick = value;
                    this.ComputeSections(this.Width, this.Height);
                    this.Invalidate();
                }
            }
        }

        public Color OffColor
        {
            get
            {
                return this.m_OffColor;
            }
            set
            {
                if (this.m_OffColor != value)
                {
                    this.m_OffColor = value;
                    this.Invalidate();
                }
            }
        }

        private void LedCtrl_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//消除锯齿！
            //填充背景
            this.m_Brush.Color = this.BackColor;
            g.FillRectangle(this.m_Brush, 0, 0, this.Height, this.Height);
            //绘制七段
            for (int i = 0; i < this.m_Sections.Length; i++)
            {
                if ((this.m_DisplayCode & (1 << i)) != 0)
                {
                    this.m_Brush.Color = this.ForeColor;
                    g.FillPolygon(this.m_Brush, this.m_Sections[i].P);
                }
                else
                {
                    //熄灭
                    if (!this.b_DrawSectionBorder)
                    {
                        this.m_Brush.Color = this.m_OffColor;
                        g.FillPolygon(this.m_Brush, this.m_Sections[i].P);
                    }
                    else
                    {
                        this.m_Pen.Color = this.m_OffColor;
                        g.DrawPolygon(this.m_Pen, this.m_Sections[i].P);
                    }
                }
            }
        }

        private void ComputeSections(int ledwidth, int ledheight)
        {
            //计算出控件中心点的坐标
            int cx = ledwidth / 2;
            int cy = ledheight / 2;

            int t1 = this.m_SectionThick * 3 / 4;	//大斜坡长
            int t2 = this.m_SectionThick / 4;	//小斜坡长
            int t3 = this.m_SectionThick / 2;	//中斜坡长
            //段的一半长度！
            int hw = cx - this.m_SectionThick - 4;	//half width of section 距离边缘2像素
            int hh = cy - this.m_SectionThick - 4;	//half height of section
            Section[] s = this.m_Sections;

            //第0段（最底下一横）
            s[0].P[0].X = cx - hw - this.m_SectionThick * 5 / 16;
            s[0].P[0].Y = cy + hh + this.m_SectionThick * 3 / 16;
            s[0].P[1].X = s[0].P[0].X - t2;
            s[0].P[1].Y = s[0].P[0].Y - t2;
            s[0].P[2].X = s[0].P[1].X + t1;
            s[0].P[2].Y = s[0].P[1].Y - t1;

            //第1段(它是中间的一横，因为和其他任何段都没对称关系，只能手写！)
            s[1].P[0].X = cx - hw + this.m_SectionThick * 5 / 16;
            s[1].P[0].Y = cy + t3;
            s[1].P[1].X = s[1].P[0].X - t3;
            s[1].P[1].Y = s[1].P[0].Y - t3;
            s[1].P[2].X = s[1].P[0].X;
            s[1].P[2].Y = cy - t3;

            //第2段(最上面一横，与第0段按y轴对称)
            for (int i = 0; i < 3; i++)
            {
                s[2].P[i].X = s[0].P[2 - i].X;
                s[2].P[i].Y = ledheight - s[0].P[2 - i].Y;
            }
            //循环为0，1，2三个水平段的p[3],p[4],p[5]赋值，注意这几个值可以根据钱三个点求出
            for (int i = 0; i < 3; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    s[i].P[j].X = ledwidth - s[i].P[5 - j].X;
                    s[i].P[j].Y = s[i].P[5 - j].Y;
                }
            }
            //到这里我们已经计算好了0，1，2段的全部坐标，下面开始计算3~6段，他们具有相互对称的关系！

            //第3段（左上的竖）(注意本身自己也不具备对称关系，6个点都要手写)
            s[3].P[0].X = cx - hw + this.m_SectionThick / 5;
            s[3].P[0].Y = cy - this.m_SectionThick * 3 / 5;
            s[3].P[1].X = s[3].P[0].X - t3;
            s[3].P[1].Y = s[3].P[0].Y + t3;
            s[3].P[2].X = s[3].P[1].X - t3;
            s[3].P[2].Y = s[3].P[1].Y - t3;
            s[3].P[3].X = s[3].P[2].X;
            s[3].P[3].Y = s[3].P[0].Y - hh + this.m_SectionThick;
            s[3].P[4].X = s[3].P[3].X + t2;
            s[3].P[4].Y = s[3].P[3].Y - t2;
            s[3].P[5].X = s[3].P[4].X + t1;
            s[3].P[5].Y = s[3].P[4].Y + t1;

            //计算4,5,6段的点坐标（4和3段x对称,5和3是y对称,6和3是原点对称）
            for (int i = 0; i < 6; i++)
            {
                int m = (8 - i) % 6;
                s[4].P[i].X = ledwidth - s[3].P[m].X;
                s[4].P[i].Y = s[3].P[m].Y;

                s[5].P[i].X = s[3].P[m].X;
                s[5].P[i].Y = ledheight - s[3].P[m].Y;

                s[6].P[i].X = ledwidth - s[3].P[i].X;
                s[6].P[i].Y = ledheight - s[3].P[i].Y;
            }
        }

        private void LedCtrl_Resize(object sender, EventArgs e)
        {
            this.ComputeSections(this.Width, this.Height);
            //m_SectionThick *= (this.Height / 42);
        }

        private void LedCtrl_SizeChanged(object sender, EventArgs e)
        {
            this.ComputeSections(this.Width, this.Height);
        }

    }
}
