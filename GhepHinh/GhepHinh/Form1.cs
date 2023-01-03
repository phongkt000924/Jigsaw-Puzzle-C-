using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace GhepHinh
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /*-----------------------------------------------------------------THUAT TOAN BFS----------------------------------------------------------------------------*/

        bool luotNguoiChoi = true;
        int buocNguoiChoi = 0;


        //2 ma tran bat bau - 1 de bai - 1 dap an (mac dinh)
        static int[,] startmx = new int[3, 3] {

            //{1,2,3},
            //{9,8,5},
            //{4,7,6}

            //{4,1,5},
            //{3,6,9},
            //{7,2,8}

            //{1,3,5},
            //{4,2,9},
            //{7,8,6}

            //{1,2,3},
            //{4,9,6},
            //{7,8,5}

            //{4,5,9},
            //{3,1,6},
            //{7,2,8}

            {1 ,2, 3},
            {4, 9, 5},
            {7, 8, 6}

        };
        static int[,] goalmx = new int[3, 3]
        {
            {1 ,2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        State temp;
        int dem;

        //2 state
        State start = new State(startmx, goalmx);
        State goal = new State(goalmx, goalmx);

        //cho biet o trong hien thi theo start dang nam o vi tri mang phan tu bao nhieu
        int node = 9;

        List<State> queue = new List<State>(); // luu cac trang thai trong luc duyet ma tran
        List<State> close = new List<State>(); // luu cac trang thai da duyet qua

        List<int> arr = new List<int>();
        int contro = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            //Tao moi truong thuc hien thuat toan

            //Tao de bai
            startmx = DeBai;
            print(startmx);
            start = new State(startmx, goalmx);

            queue = new List<State>();
            close = new List<State>();

            arr = new List<int>();
            contro = 0;

            /*
             1 len
             2 xuong
             3 trai 
             4 phai
             */
            luotNguoiChoi = false;
            queue.Add(start); //them
            start.buoc = 0;
            dem = 0;

            //vao vong while thuat toan
            while (queue.Count > 0) //check xem hang doi con phan tu nao khong?
            {
                //lay phan tu dau tien trong queue ra
                temp = queue.ElementAt(0);
                queue.RemoveAt(0);

                //them vao close
                close.Add(temp);

                //ham dem so lan lap
                dem++;
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Buoc: " + dem);
                print(temp.matrix);

                //kiem tra xem cai temp co phai la trang thai dich (goal) hay khong?
                if (check(temp.matrix, goal.matrix)) {
                    Console.WriteLine();
                    Console.WriteLine("==> 2 ma tran giong nhau!");

                    Console.WriteLine("###########################################################\r\n");

                    Console.WriteLine();
                    timCha(temp);
                    Console.WriteLine(arr.Count);
                    contro = arr.Count;
                    lb_tongBuoc.Text = arr.Count.ToString();
                    lb_buocHT.Text = "0";
                    lb_thongbao.Text = "Trò chơi đã được giải bởi thuật toán BFS!\r\nBấm chơi lại để chơi lại!!!";
                    Console.WriteLine("\r\n###########################################################");

                    Console.WriteLine();
                    print(temp.matrix);

                    return;
                }

                //tim vi tri o khuyet tren State hien tai la temp
                int nodei = 0, nodej = 0;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (temp.matrix[i, j] == node)
                        {
                            nodei = i;
                            nodej = j;
                            break;
                        }
                    }
                }

                foreach (State st in find(nodei, nodej, temp.matrix)) {
                    /*duyet cac trang thai v=st ke voi u=temp. 
                     * kiem tra xem co ton tai trong queue va close hay khong? */

                    bool ck = false; //bien check

                    //kiem tra trong queue, neu thuoc queue thi khong ktra trong close
                    foreach (State statetemp in queue) {
                        if (check(statetemp.matrix, st.matrix))
                        {
                            ck = true; //co trong queue
                            break;
                        }
                    }

                    //check trong close
                    if (ck == false) //kiem tra xem da co trong queue neu co roi thi bo qua close
                    {
                        foreach (State statetemp in close) {
                            if (check(statetemp.matrix, st.matrix))
                            {
                                ck = true; //co trong queue va close
                                break;
                            }
                        }
                    }
                    if (ck == false)
                    /*them v=st vao cuoi queue*/
                    {
                        st.parent = temp; //cap nhat cha cua st = temp
                        st.haveParent = true;
                        queue.Add(st);
                    }
                }
                queue.Sort(); //sx cac trang thai trong queue, sao do quay lai while thuc như vay cho den khi tim ra
            }
            Console.WriteLine("Ket thuc vong lap for va khong duoc gi!");
        }


        //kiem tra 2 mang co giong nhau ko? --> giong true, khac false
        bool check(int[,] a, int[,] b)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (a[i, j] != b[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //tra ve 1 mang, giong voi mang truyen vao
        int[,] copy(int[,] test)
        {
            int[,] arr = new int[3, 3];

            for (int i1 = 0; i1 < 3; i1++)
            {
                for (int j1 = 0; j1 < 3; j1++)
                {
                    arr[i1, j1] = test[i1, j1];
                }
            }

            return arr;
        }

        //ham in ma tran
        void print(int[,] test)
        {
            for (int i1 = 0; i1 < 3; i1++)
            {
                for (int j1 = 0; j1 < 3; j1++)
                {
                    Console.Write(test[i1, j1]);
                }
                Console.WriteLine();
            }
        }

        //tim cac trang thai co the co the xay ra di chuyen tu State
        List<State> find(int i, int j, int[,] matrix)
        {
            /*
             1 len
             2 xuong
             3 trai 
             4 phai
             */

            List<State> listReturn = new List<State>();

            //check di len
            if (i - 1 >= 0)
            {
                int[,] test = copy(matrix);
                int t = test[i, j];
                test[i, j] = test[i - 1, j];
                test[i - 1, j] = t;
                listReturn.Add(new State(test, goalmx, 1));
            }

            //check di xuong
            if (i + 1 <= 2)
            {
                int[,] test = copy(matrix);
                int t = test[i, j];
                test[i, j] = test[i + 1, j];
                test[i + 1, j] = t;
                listReturn.Add(new State(test, goalmx, 2));
            }

            //check qua trai
            if (j - 1 >= 0)
            {
                int[,] test = copy(matrix);
                int t = test[i, j];
                test[i, j] = test[i, j - 1];
                test[i, j - 1] = t;
                listReturn.Add(new State(test, goalmx, 3));
            }

            //check qua phai
            if (j + 1 <= 2)
            {
                int[,] test = copy(matrix);
                int t = test[i, j];
                test[i, j] = test[i, j + 1];
                test[i, j + 1] = t;
                listReturn.Add(new State(test, goalmx, 4)); ;
            }
            return listReturn;
        }

        //in lai buoc di cua o khuyet
        void timCha(State a)
        {
            if (a.buoc == 0)
            {
                Console.WriteLine(a.buoc);
                return;
            }
            else {
                arr.Add(a.buoc);
            }
            Console.WriteLine(a.buoc);
            timCha(a.parent);
        }


        /*-----------------------------------------------------------------THUAT TOAN BFS----------------------------------------------------------------------------*/

        /*-----------------------------------------------------------------HIEN THI----------------------------------------------------------------------------*/

        PictureBox[,] picture = new PictureBox[3, 3];
        Point oTrong = new Point(0, 0);
        static int[,] DeBai = new int[3, 3]
        {
            {1 ,2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };

        void taoBanCo()
        {
            pnKhung.Controls.Clear();
            DeBai = new int[3, 3]
            {
                {1 ,2, 3},
                {4, 5, 6},
                {7, 8, 9}
            };
            buocNguoiChoi = 0;
            lb_buocnguoichoi.Text = buocNguoiChoi+"";
            int count = 1;
            int Size = 150;
            Bitmap img = ChuyenDoiKichThuocAnh(Image.FromFile(@"E:\Phong_Project\DotNet\GhepHinh\anhdaden.jpg"), Size * 3, Size * 3);
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.ShowDialog();
            //Bitmap img = ChuyenDoiKichThuocAnh(Image.FromFile(ofd.FileName), Size * 3, Size * 3);
            pictureBox1.Image = img;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Bitmap anhnho = img.Clone(new Rectangle(j * Size, i * Size, Size, Size), PixelFormat.Format32bppArgb);
                    PictureBox pictureBox = new PictureBox()
                    {
                        Size = new Size(Size, Size),
                        Location = new Point(j * Size + j * 5, i * Size + i * 5),
                        BackColor = Color.Blue,
                        Image = anhnho,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Tag = new Point(j, i)
                    };
                    if (count == 9)
                    {
                        Graphics g = Graphics.FromImage(pictureBox.Image);
                        g.FillRectangle(new SolidBrush(Color.Green), new Rectangle(0, 0, Size, Size));
                        oTrong = new Point(j, i);
                    }
                    count++;
                    pictureBox.Click += PictureBox_Click;
                    pnKhung.Controls.Add(pictureBox);
                    picture[j, i] = pictureBox;
                }
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (!luotNguoiChoi) return;
            PictureBox a = (PictureBox)sender;
            Point p = (Point)a.Tag;

            if (p.X == oTrong.X && p.Y == oTrong.Y - 1)
            {
                buocNguoiChoi++;
                lb_buocnguoichoi.Text = buocNguoiChoi + "";
                diChuyen(1);
            }
            else if (p.X == oTrong.X && p.Y == oTrong.Y + 1)
            {
                buocNguoiChoi++;
                lb_buocnguoichoi.Text = buocNguoiChoi + "";
                diChuyen(2);
            }
            else if (p.X == oTrong.X - 1 && p.Y == oTrong.Y)
            {
                buocNguoiChoi++;
                lb_buocnguoichoi.Text = buocNguoiChoi + "";
                diChuyen(3);
            }
            else if (p.X == oTrong.X + 1 && p.Y == oTrong.Y)
            {
                buocNguoiChoi++;
                lb_buocnguoichoi.Text = buocNguoiChoi + "";
                diChuyen(4);
            }

            if (check(goalmx, DeBai))
            {
                MessageBox.Show("Chúc mừng bạn thật là tài giỏi !!! \r\nBạn đã hoàn thành trò chơi với "+buocNguoiChoi+" Bước");
                button8_Click(null,null);
            }
        }

        void diChuyen(int huong)
        {
            /*
             1 len
             2 xuong
             3 trai 
             4 phai
             */

            if (huong == 1)
            {
                if (kiemTraDKDiChuyen(oTrong.X, oTrong.Y - 1))
                {
                    doiAnh(new Point(oTrong.X, oTrong.Y), new Point(oTrong.X, oTrong.Y - 1));
                }
            } else if (huong == 2)
            {
                if (kiemTraDKDiChuyen(oTrong.X, oTrong.Y + 1))
                {
                    doiAnh(new Point(oTrong.X, oTrong.Y), new Point(oTrong.X, oTrong.Y + 1));
                }

            } else if (huong == 3)
            {
                if (kiemTraDKDiChuyen(oTrong.X - 1, oTrong.Y))
                {
                    doiAnh(new Point(oTrong.X, oTrong.Y), new Point(oTrong.X - 1, oTrong.Y));
                }

            } else if (huong == 4)
            {
                if (kiemTraDKDiChuyen(oTrong.X + 1, oTrong.Y))
                {
                    doiAnh(new Point(oTrong.X, oTrong.Y), new Point(oTrong.X + 1, oTrong.Y));
                }

            }
        }

        bool kiemTraDKDiChuyen(int x, int y)
        {
            if (x > 2 || x < 0 || y > 2 || y < 0)
            {
                return false;
            }
            return true;
        }

        void doiAnh(Point a, Point b)
        {
            var temp = picture[a.X, a.Y].Image;
            picture[a.X, a.Y].Image = picture[b.X, b.Y].Image;
            picture[b.X, b.Y].Image = temp;

            var temp2 = DeBai[a.Y, a.X];
            DeBai[a.Y, a.X] = DeBai[b.Y, b.X];
            DeBai[b.Y, b.X] = temp2;

            oTrong = b;
        }

        void tronBanCo(int n1)
        {
            int solan = n1;
            int buocVuaDi = 0;
            while(solan > 0)
            {
                solan--;
                List<int> BuocCoTheDi = new List<int>();
                if (oTrong.Y - 1 >= 0)
                {
                    if (daoNguoc(buocVuaDi) != 1)
                        BuocCoTheDi.Add(1);
                }
                 if (oTrong.Y + 1 <= 2)
                {
                    if (daoNguoc(buocVuaDi) != 2)
                        BuocCoTheDi.Add(2);
                }
                 if (oTrong.X - 1 >= 0)
                {
                    if (daoNguoc(buocVuaDi) != 3)
                        BuocCoTheDi.Add(3);
                }
                if (oTrong.X + 1 <= 2)
                {
                    if (daoNguoc(buocVuaDi) != 4)
                        BuocCoTheDi.Add(4);
                }

                //Tron danh sach buoc co the di
                Random rng = new Random();
                int n = BuocCoTheDi.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    int value = BuocCoTheDi[k];
                    BuocCoTheDi[k] = BuocCoTheDi[n];
                    BuocCoTheDi[n] = value;
                }
                //MessageBox.Show(BuocCoTheDi[0].ToString());
                buocVuaDi= BuocCoTheDi[0];
                diChuyen(BuocCoTheDi[0]);
            }
            if (check(goalmx, DeBai))
            {
                tronBanCo(n1);
            }
        }

       

        /*Nhan vao 1 tam hinh, chieu dai, chieu rong. Tra ra 1 tam hinh co kich thuoc co chieu dai chieu rong luc nhap vao.*/
        Bitmap ChuyenDoiKichThuocAnh(Image img, int width, int height)
        {
            Rectangle khungAnh = new Rectangle(0, 0, width, height);
            Bitmap anhTraVe = new Bitmap(width, height);

            anhTraVe.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            var graphics = Graphics.FromImage(anhTraVe);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var wrapMode = new ImageAttributes();
            wrapMode.SetWrapMode(WrapMode.TileFlipXY);

            graphics.DrawImage(img, khungAnh, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);

            return anhTraVe;
        }

        /*-----------------------------------------------------------------HIEN THI----------------------------------------------------------------------------*/

        /*-----------------------------------------------------------------THAO TAC NGUOI CHOI VOI GIAO DIEN----------------------------------------------------------------------------*/
        private void button8_Click(object sender, EventArgs e)
        {
            taoBanCo();
            luotNguoiChoi = true;
            lb_thongbao.Text = "Chúc bạn may mắn !!!";
            tronBanCo(3);
        }

        //Di toi
        private void button4_Click(object sender, EventArgs e)
        {
            if (contro - 1 < 0)
            {
                return;
            } else
            {
                contro--;
                diChuyen(arr[contro]);
                lb_buocHT.Text = (arr.Count - contro).ToString();
            }
        }

        //Di lui
        private void button5_Click(object sender, EventArgs e)
        {
            if (contro + 1 > arr.Count)
            {
                return;
            }
            else
            {
                diChuyen(daoNguoc(arr[contro]));
                contro++;
                lb_buocHT.Text = (arr.Count - contro).ToString();
            }
        }

        int daoNguoc(int huong)
        {
            if (huong == 0) return 0;
            if (huong == 1)
                return 2;
            else if (huong == 2)
                return 1;
            else if (huong == 3)
                return 4;
            else return 3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button8_Click(null, null);
        }
    }
}
