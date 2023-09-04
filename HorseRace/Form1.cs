using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ProgressBar = System.Windows.Forms.ProgressBar;

namespace HorseRace
{
    public partial class Form1 : Form
    {

        public SynchronizationContext uiContext;

        List<ProgressBar> pb = null;
     //   List<Label> names = null;
        Random rand = new Random();
        int counter = 1;//когда лошадь добегает то в лист бокс записывается ее номер 

        public Form1()
        {
            InitializeComponent();
            this.Text = "Horse Racing";
            uiContext = SynchronizationContext.Current;
            pb = new List<ProgressBar> { progressBar1, progressBar2, progressBar3, progressBar4, progressBar5 };
          //  names=new List<Label>(){label1,label2, label3, label4, label5};
            progressBar1.Name = "Horse 1";
            progressBar2.Name = "Horse 2";
            progressBar3.Name = "Horse 3";
            progressBar4.Name = "Horse 4";
            progressBar5.Name = "Horse 5";

        }
        public void ThreadFunk(object obj)
        {
            ProgressBar pb2 = (ProgressBar)obj;


            uiContext.Send(d => pb2.Minimum = 0, null);
            uiContext.Send(d => pb2.Maximum = 230, null);
            uiContext.Send(d => pb2.Value = 0, null);

            int count = 0; //  значение прогресс Бара        
                while (true)
                {
                    count += rand.Next(1, 15);
                    if (count <= pb2.Maximum)
                        uiContext.Send(d => pb2.Value = count, null);
                    else
                    {
                    ListViewItem item = new ListViewItem(pb2.Name);
                    item.SubItems.Add(counter.ToString());
                      uiContext.Send(d => pb2.Value = count - (count - pb2.Maximum), null);
                        uiContext.Send(d => listView1.Items.Add(item), null);
                        counter++;
                        break;
                    }
                    Thread.Sleep(100);


                }
            }

        

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //foreach (var t in pb)
            //    t.Abort();
        }
        
        private void progressBar5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)///START
        {
            button1.Enabled = false;

            foreach (var t in pb)// Создание и запуск потоков для каждого ProgressBar
            {
                Thread thread = new Thread(new ParameterizedThreadStart(ThreadFunk));
                thread.IsBackground = true;
                thread.Start(t);

                Thread.Sleep(100);
            }

        }
    }
}



