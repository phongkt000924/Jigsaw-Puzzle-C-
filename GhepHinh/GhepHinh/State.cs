using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhepHinh
{
    class State : IComparable {

        public int[,] matrix { get; set; } //ma tran cua trang thai hien tai chinh la state
        public State parent { get; set; } //cha cua state
        public bool haveParent { get; set; } // kiem tra xem nut co cha hay khong?
        public int oSai { get; set; } //so luong o nam sai vi tri so voi state goal
        public int [,] goal { get; set;} //mang ket qua mong muon de so sanh va tim ra so luong o sai
        public int buoc { get; set; } //thao tac thuc hien tu buoc truoc do sang state nay

        public State(int[,] matrix, int[,] goal)
        {
            this.matrix = matrix;
            this.goal = goal;
            parent = null;
            haveParent = false;
            oSai = tinhOSai();
        }

        public State(int[,] matrix, int[,] goal, int buoc) : this(matrix, goal)
        {
            this.buoc = buoc;
            oSai = tinhOSai();
        }

        public State()
        {
        }

        //dem so luong o sai so voi ma tran ket qua
        private int  tinhOSai()
        {
            int count = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[i,j] != goal[i,j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        //So sanh 2 state voi nhau thong qua cach tra lai gia tri
        public int CompareTo(object obj) 
        {
            State t = (State)obj;
            if (t.oSai < this.oSai) {
                return 1;
            } else if (t.oSai > this.oSai)
            {
                return -1;
            } else
            {
                return 0;
            }
        }

    }
}
