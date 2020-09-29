using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool is_english = false;

        int tab_count = 5;

        int half_tabs;


        Ping[] icmp;

        AutoResetEvent[] waiter;

        PingReply[] reply;



        Grid[] grid;

        GroupBox[] gb;
        CheckBox[] cb;
        Button[] ping;
        Button[] stnx;
        Timer[] timer;
        DataGrid[] dtgr;

        DataGridTextColumn[] colN;
        DataGridTextColumn[] colD;
        DataGridTextColumn[] colI;
        DataGridTextColumn[] colT;
        DataGridTextColumn[] colS;

        public MainWindow()
        {
            InitializeComponent();

            half_tabs = tab_count / 2;

            InitControls();
            Translate();
            //timer1.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            label1.Content = "Hi!";
            
            for (int i = 0; i < 100; i++)
            {
                Button button = new Button();
                MainGrid.Children.Add(button);
            }
        }

        private void InitControls()
        {
            grid = new Grid[tab_count];

            gb = new GroupBox[tab_count];
            cb = new CheckBox[tab_count];
            ping = new Button[tab_count];
            stnx = new Button[tab_count];
            timer = new Timer[tab_count];
            dtgr = new DataGrid[tab_count];
            
            colN = new DataGridTextColumn[tab_count];
            colD = new DataGridTextColumn[tab_count];
            colI = new DataGridTextColumn[tab_count];
            colT = new DataGridTextColumn[tab_count];
            colS = new DataGridTextColumn[tab_count];



            for (int i = 0; i < tab_count; i++)
            {
                grid[i] = new Grid();
                MainGrid.Children.Add(grid[i]);
                //CreateChild(grid[i], MainGrid);

                grid[i].HorizontalAlignment = HorizontalAlignment.Left;
                grid[i].VerticalAlignment = VerticalAlignment.Top;
                grid[i].Margin = new Thickness(10 + 440 * i, 100, 0, 0);
                grid[i].Width = 435;
                grid[i].Height = 315;
            }

            for (int i = 0; i < tab_count; i++)
            {
                gb[i] = new GroupBox();
                grid[i].Children.Add(gb[i]);

                cb[i] = new CheckBox();
                grid[i].Children.Add(cb[i]);

                ping[i] = new Button();
                grid[i].Children.Add(ping[i]);

                stnx[i] = new Button();
                grid[i].Children.Add(stnx[i]);

                dtgr[i] = new DataGrid();
                grid[i].Children.Add(dtgr[i]);

                colN[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colN[i]);

                colD[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colD[i]);

                colI[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colI[i]);

                colT[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colT[i]);

                colS[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colS[i]);
            }

            for (int i = 0; i < tab_count; i++)
            {
                //gb[i].Location = (i < half_tabs) ? new Point(10 + i * (830 / half_tabs), 85) : new Point(10 + (i - half_tabs) * (830 / (half_tabs + 1)), 90 + gb[i].Size.Height);
                //gb[i].Location = (i <= half_tabs) ? new Point(10 + i * 445, 10) : new Point((i * 450) - (tab_count / 2 * 445 - 10), 335);
                //gb[i].Name = "gb" + i.ToString();
                //gb[i].Size = (i < half_tabs) ? new Size((830 - half_tabs * 10) / half_tabs, (ClientSize.Height - 96) / 2) : new Size((830 - half_tabs * 10 - 10) / (half_tabs + 1), (ClientSize.Height - 96) / 2);
                //gb[i].Size = (i <= half_tabs) ?     new Size(435, 315)          : new Size((ClientSize.Width - 30) / 2, 315);
                //gb[i].TabIndex = i;
                //gb[i].Text = "";
                //gb[i].Controls.Add(cb[i]);
                //gb[i].Controls.Add(ping[i]);
                //gb[i].Controls.Add(stnx[i]);
                //gb[i].Controls.Add(grid[i]);
                //Controls.Add(gb[i]);



                cb[i].HorizontalAlignment = HorizontalAlignment.Left;
                cb[i].VerticalAlignment = VerticalAlignment.Top;
                cb[i].Margin = new Thickness(10, 20, 0, 0); // new Point(11, 20)"11,20,0,0"
                cb[i].Name = "cb" + i.ToString();
                cb[i].Width = 15;
                cb[i].Height = 14;
                cb[i].TabIndex = 10 + i;
                cb[i].Checked += new RoutedEventHandler(AutopingClick);

                ping[i].HorizontalAlignment = HorizontalAlignment.Left;
                ping[i].VerticalAlignment = VerticalAlignment.Top;
                ping[i].Margin = new Thickness(5, 45, 0, 0);
                ping[i].Name = "ping" + i.ToString();
                ping[i].Width = 150;
                ping[i].Height = 30;
                ping[i].TabIndex = 20 + i;
                ping[i].Click += new RoutedEventHandler(PingClick);

                //stnx[i] = new Button();
                stnx[i].HorizontalAlignment = HorizontalAlignment.Left;
                stnx[i].VerticalAlignment = VerticalAlignment.Top;
                stnx[i].Margin = new Thickness(280, 45, 0, 0);
                stnx[i].Name = "stnx" + i.ToString();
                stnx[i].Width = 150;
                stnx[i].Height = 30;
                stnx[i].TabIndex = 30 + i;
                stnx[i].Click += new RoutedEventHandler(SettingsClick);

                //timer[i] = new Timer();
                //timer[i].Tick += new EventHandler(TimerTick);



                //colN[i] = new DataGridTextColumn();
                //colN[i].FillWeight = 150F;
                colN[i].Header = "Name";
                colN[i].MinWidth = 25;
                //colN[i].Name = "colN" + i.ToString();
                colN[i].CanUserReorder = false;
                colN[i].CanUserResize = false;
                colN[i].CanUserSort = false;
                colN[i].IsReadOnly = true;
                colN[i].Width = 205;

                //colD[i] = new DataGridTextColumn();
                colD[i].Header = "DNS";
                colD[i].MinWidth = 25;
                //colD[i].Name = "colD" + i.ToString();
                colD[i].CanUserReorder = false;
                colD[i].CanUserResize = false;
                colD[i].CanUserSort = false;
                colD[i].IsReadOnly = true;

                //colI[i] = new DataGridTextColumn();
                colI[i].Header = "IP";
                colI[i].MinWidth = 25;
                //colI[i].Name = "colI" + i.ToString();
                colI[i].CanUserReorder = false;
                colI[i].CanUserResize = false;
                colI[i].CanUserSort = false;
                colI[i].IsReadOnly = true;
                colI[i].Width = 150;

                //colT[i] = new DataGridTextColumn();
                colT[i].Header = "Time";
                colT[i].MinWidth = 25;
                //colT[i].Name = "colT" + i.ToString();
                colT[i].CanUserReorder = false;
                colT[i].CanUserResize = false;
                colT[i].CanUserSort = false;
                colT[i].IsReadOnly = true;
                colT[i].Width = 125;

                //colS[i] = new DataGridTextColumn();
                colS[i].Header = "Status";
                colS[i].MinWidth = 25;
                //colS[i].Name = "colS" + i.ToString();
                colS[i].CanUserReorder = false;
                colS[i].CanUserResize = false;
                colS[i].CanUserSort = false;
                colS[i].IsReadOnly = true;
                colS[i].Width = 75;



                /*DataGridCellStyle style1 = new DataGridViewCellStyle();
                style1.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);

                DataGridViewCellStyle style2 = new DataGridViewCellStyle();
                style2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);*/

                dtgr[i].HorizontalAlignment = HorizontalAlignment.Left;
                dtgr[i].VerticalAlignment = VerticalAlignment.Top;
                //dtgr[i] = new DataGrid();
                /*grid[i].AllowUserToAddRows = false;
                grid[i].AllowUserToDeleteRows = false;
                grid[i].AllowUserToResizeColumns = false;
                grid[i].AllowUserToResizeRows = false;
                grid[i].AlternatingRowsDefaultCellStyle = style1;
                grid[i].CausesValidation = false;
                grid[i].ColumnHeadersDefaultCellStyle = style2;
                grid[i].ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                grid[i].Columns.AddRange(new DataGridViewTextBoxColumn[] { colN[i], colD[i], colI[i], colT[i], colS[i] });
                grid[i].EditMode = DataGridViewEditMode.EditProgrammatically;*/
                dtgr[i].Margin = new Thickness(5, 80, 0, 0);
                //grid[i].MultiSelect = false;
                dtgr[i].Name = "grid" + i.ToString();
                dtgr[i].IsReadOnly = true;
                //grid[i].RowHeadersVisible = false;
                //grid[i].RowHeadersWidth = 51;
                //grid[i].RowTemplate.Height = 25;
                //grid[i].SelectionMode = DataGridViewSelectionMode.CellSelect;
                //grid[i].ShowCellErrors = false;
                //grid[i].ShowEditingIcon = false;
                //grid[i].ShowRowErrors = false;
                dtgr[i].Width = 425;
                dtgr[i].Height = 230;
                //grid[i].StandardTab = true;
                dtgr[i].TabIndex = 40 + i;
                //dtgr[i].Rows.Add(5);
                dtgr[i].MouseDoubleClick += new MouseButtonEventHandler(GridCellClick);
            }

            label1.Content = Width.ToString();

            //ResizeForm();
        }

        private void Translate()
        {
            if (is_english)
            {
                /*toolStripButton1.Text = "File";
                Open_iniTSMitem.Text = "Open .INI file";
                Save_iniTSMitem.Text = "Save .INI file";
                Open_logTSMitem.Text = "Open log file";
                Open_clientsTSMitem.Text = "Open clients list";
                button0.Text = "Ping all";
                toolStripButton3.Text = "Tracking";
                toolStripButton4.Text = "Settings";
                LanguageTSMitem.Text = "Language";
                Lang_rusTSMitem.Text = "Russian";
                Lang_engTSMitem.Text = "English";
                toolStripButton5.Text = "Help";
                User_guideTSMitem.Text = "User's guide";
                AboutTSMitem.Text = "About";*/

                for (int i = 0; i < tab_count; i++)
                {
                    colN[i].Header = "Name";
                    colT[i].Header = "Time";
                    colS[i].Header = "Status";

                    cb[i].Content = "Autoping " + (i + 1) + " group";

                    stnx[i].Content = "Settings";

                    ping[i].Content = "Ping " + (i + 1) + " group";
                }

                /*pinging = "Pinging...";
                check_connection = "No connection to the network." + Environment.NewLine + "Check cable connection or network/firewall settings.";
                fill_clients_list = "Fill clients list.";*/
            }
            else
            {
                /*toolStripButton1.Text = "Файл";
                Open_iniTSMitem.Text = "Открыть .INI файл";
                Save_iniTSMitem.Text = "Сохранить .INI файл";
                Open_logTSMitem.Text = "Открыть лог файл";
                Open_clientsTSMitem.Text = "Открыть список клиентов";
                button0.Text = "Опрос всех абонентов";
                toolStripButton3.Text = "Слежение";
                toolStripButton4.Text = "Настройки";
                LanguageTSMitem.Text = "Язык";
                Lang_rusTSMitem.Text = "Русский";
                Lang_engTSMitem.Text = "Английский";
                toolStripButton5.Text = "Помощь";
                User_guideTSMitem.Text = "Руководство пользователя";
                AboutTSMitem.Text = "О программе";*/

                for (int i = 0; i < tab_count; i++)
                {
                    colN[i].Header = "Имя";
                    colT[i].Header = "Время";
                    colS[i].Header = "Статус";

                    cb[i].Content = "Автоматический опрос " + (i + 1) + " группы";

                    stnx[i].Content = "Настройки";

                    ping[i].Content = "Опрос " + (i + 1) + " группы";
                }

                /*pinging = "Опрос...";
                check_connection = "Нет подключения к сети" + Environment.NewLine + "Проверьте подключение сетевого кабеля или настройки сети/фаерволла";
                fill_clients_list = "Заполните список клиентов.";*/
            }
        }

        private void Func()
        {
            for (int i = 0; i < tab_count; i++)
            {
                Random rnd = new Random();
                int value = rnd.Next(1, 4);

                switch (value)
                {
                    case 1:
                        /*pbx[i].Image = Properties.Resources.stop32;
                        ttl[i].Text = "Some STOP text";
                        txt[i].Text = "Начало события: " + DateTime.Now;*/
                        break;
                    case 2:
                        /*pbx[i].Image = Properties.Resources.error32;
                        ttl[i].Text = "Some ERROR text";
                        txt[i].Text = "Начало события: " + DateTime.Now;*/
                        break;
                    case 3:
                        /*pbx[i].Image = Properties.Resources.info32;
                        ttl[i].Text = "Some INFO text";
                        txt[i].Text = "Начало события: " + DateTime.Now;*/
                        break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Func();
        }

        private void AutopingClick(object sender, EventArgs e)
        {
            int group = Method(sender, cb);

            label1.Content = "Autoping " + (group + 1);

            //CheckChange(grid[group], timer[group], cb[group], (group + 1), g_settings[group, 0]);
        }

        private void PingClick(object sender, EventArgs e)
        {
            int group = Method(sender, ping);

            label1.Content = "Ping " + (group + 1);

            //if (timer[group].Enabled)
            //    timer[group].Stop();

            //button0.Enabled = false;
            //ping_completed[group] = false;

            //ClearGrid(grid[group], g_settings[group, 0]);
            //SortPing(group + 1);
        }

        private void SettingsClick(object sender, EventArgs e)
        {
            int group = Method(sender, stnx);

            label1.Content = "Settings " + (group + 1);

            //ShowSettings(group + 1);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            int group = Method(sender, timer);

            /*button0.Enabled = false;
            ping_completed[group] = false;

            ClearGrid(grid[group], g_settings[group, 0]);
            SortPing(group + 1);*/
        }

        private void GridCellClick(object sender, MouseEventArgs e)
        {
            int group = Method(sender, dtgr);

            label1.Content = "Grid " + (group + 1);

            //ShowTracking(grid[group], group);
        }

        /*private void Received_reply(object sender, PingCompletedEventArgs e)
        {
            if (e.Cancelled)
                ((AutoResetEvent)e.UserState).Set();

            if (e.Error != null)
                ((AutoResetEvent)e.UserState).Set();

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();

            int group = Method(sender, ping);

            reply[group] = e.Reply;

            if (!to_close)
                SortReply(group + 1);
        }*/

        private int Method(object sender, object[] compare_with)
        {
            int result;

            for (result = 0; result < tab_count; result++)
                if (sender.Equals(compare_with[result]))
                    break;

            return result;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void ResizeForm()
        {
            int indent = half_tabs * 10;
            int count = (tab_count % 2 != 0) ? half_tabs + 1 : half_tabs;

            int WorkZone_width = (int)Width - 10,
                WorkZone_sizew = (tab_count % 2 != 0) ? (int)Width - 10 - indent : (int)Width - 20 - indent,
                WorkZone_height = (int)Height - 96;

            for (int i = 0; i < tab_count; i++)
            {
                int sizew = (i < half_tabs) ? WorkZone_sizew / half_tabs : WorkZone_sizew / count,
                    sizeh = WorkZone_height / 2;

                int locx = (i < half_tabs) ? 10 + i * (WorkZone_width / half_tabs) : 10 + (i - half_tabs) * (WorkZone_width / count),
                    locy = (i < half_tabs) ? 85 : 90 + sizeh;



                gb[i].Width = sizew;
                gb[i].Width = sizeh;
                
                //gb[i].Location = new Point(locx, locy);

                //grid[i].Size = new Size(sizew - 10, sizeh - 85);

                //stnx[i].Location = new Point(sizew - 155, 45);



                //gb[i].Size = (i < half_tabs) ? new Size((WorkZone_width - half_tabs * 10) / half_tabs, (ClientSize.Height - 96) / 2) : new Size((WorkZone_width - half_tabs * 10 - 10) / (half_tabs + 1), (ClientSize.Height - 96) / 2);

                //gb[i].Location = (i < half_tabs) ? new Point(10 + i * (WorkZone_width / half_tabs), 85) : new Point(10 + (i - half_tabs) * (WorkZone_width / (half_tabs + 1)), 90 + gb[i].Size.Height);
            }
        }
    }
}
