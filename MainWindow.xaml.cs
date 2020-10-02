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

        int tab_count = 4;

        // первая половина количества групп без округления / вторая половина количества групп с округлением в большую сторону
        int first_half, second_half;

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

            first_half = tab_count / 2;
            second_half = (tab_count % 2 != 0) ? first_half + 1 : first_half;

            MinWidth = 330 * second_half;
            MinHeight = 400;

            InitGrids();
            InitControls();
            Translate();
            //timer1.Start();
        }

        private void InitGrids()
        {
            grid = new Grid[tab_count];

            for (int i = 0; i < tab_count; i++)
            {
                grid[i] = new Grid();
                GridGroup.Children.Add(grid[i]);

                grid[i].HorizontalAlignment = HorizontalAlignment.Left;
                grid[i].VerticalAlignment = VerticalAlignment.Top;
                grid[i].Margin = new Thickness(10 + 440 * i, 100, 0, 0);
                grid[i].Width = 435;
                grid[i].Height = 315;
            }
        }

        private void InitControls()
        {
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
                gb[i] = new GroupBox();
                grid[i].Children.Add(gb[i]);

                gb[i].FontWeight = FontWeights.Bold;

                cb[i] = new CheckBox();
                grid[i].Children.Add(cb[i]);

                cb[i].HorizontalAlignment = HorizontalAlignment.Left;
                cb[i].VerticalAlignment = VerticalAlignment.Top;
                cb[i].Margin = new Thickness(5, 25, 0, 0);
                cb[i].Width = 200;
                cb[i].Height = 15;
                cb[i].Checked += new RoutedEventHandler(AutopingClick);

                ping[i] = new Button();
                grid[i].Children.Add(ping[i]);

                ping[i].HorizontalAlignment = HorizontalAlignment.Left;
                ping[i].VerticalAlignment = VerticalAlignment.Top;
                ping[i].Margin = new Thickness(5, 45, 0, 0);
                ping[i].Width = 150;
                ping[i].Height = 30;
                ping[i].Click += new RoutedEventHandler(PingClick);

                stnx[i] = new Button();
                grid[i].Children.Add(stnx[i]);

                stnx[i].HorizontalAlignment = HorizontalAlignment.Right;
                stnx[i].VerticalAlignment = VerticalAlignment.Top;
                stnx[i].Margin = new Thickness(0, 45, 5, 0);
                stnx[i].Width = 150;
                stnx[i].Height = 30;
                stnx[i].Click += new RoutedEventHandler(SettingsClick);



                dtgr[i] = new DataGrid();
                grid[i].Children.Add(dtgr[i]);

                /*dtgr[i].AllowUserToAddRows = false;
                dtgr[i].AllowUserToDeleteRows = false;
                dtgr[i].AllowUserToResizeColumns = false;
                dtgr[i].AllowUserToResizeRows = false;
                dtgr[i].AlternatingRowsDefaultCellStyle = style1;
                dtgr[i].CausesValidation = false;
                dtgr[i].ColumnHeadersDefaultCellStyle = style2;
                dtgr[i].ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dtgr[i].Columns.AddRange(new DataGridViewTextBoxColumn[] { colN[i], colD[i], colI[i], colT[i], colS[i] });
                dtgr[i].EditMode = DataGridViewEditMode.EditProgrammatically;*/
                dtgr[i].Margin = new Thickness(5, 80, 5, 5);
                //dtgr[i].MultiSelect = false;
                //dtgr[i].Name = "grid" + i.ToString();
                //dtgr[i].IsReadOnly = true;
                //dtgr[i].RowHeadersVisible = false;
                //dtgr[i].RowHeadersWidth = 51;
                //dtgr[i].RowTemplate.Height = 25;
                //dtgr[i].SelectionMode = DataGridViewSelectionMode.CellSelect;
                //dtgr[i].ShowCellErrors = false;
                //dtgr[i].ShowEditingIcon = false;
                //dtgr[i].ShowRowErrors = false;
                //dtgr[i].Width = 425;
                //dtgr[i].Height = 230;
                //dtgr[i].StandardTab = true;
                //dtgr[i].TabIndex = 40 + i;
                //dtgr[i].Rows.Add(5);
                //dtgr[i].Items.Add();
                dtgr[i].MouseDoubleClick += new MouseButtonEventHandler(GridCellClick);

                colN[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colN[i]);
                SetColumnStyle(colN[i], 205, false);

                colD[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colD[i]);
                SetColumnStyle(colD[i], 150, true);

                colI[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colI[i]);
                SetColumnStyle(colI[i], 150, true);

                colT[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colT[i]);
                SetColumnStyle(colT[i], 125, true);

                colS[i] = new DataGridTextColumn();
                dtgr[i].Columns.Add(colS[i]);
                SetColumnStyle(colS[i], 0, false);

                //timer[i] = new Timer();
                //timer[i].Tick += new EventHandler(TimerTick);



                /*DataGridCellStyle style1 = new DataGridViewCellStyle();
                style1.Font = new Font("Microsoft Sans Serif", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);

                DataGridViewCellStyle style2 = new DataGridViewCellStyle();
                style2.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 204);*/
            }
        }

        private void SetColumnStyle(DataGridTextColumn column, double width, bool visibility)
        {
            column.MinWidth = 25;
            column.Visibility = (visibility) ? Visibility.Hidden : Visibility.Visible;
            column.CanUserReorder = false;
            column.CanUserResize = false;
            column.CanUserSort = false;
            column.IsReadOnly = true;
            column.Width = (width != 0) ? width : new DataGridLength(75, DataGridLengthUnitType.Star);
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

                    gb[i].Header = "Group " + (i + 1);
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

                    gb[i].Header = "Группа " + (i + 1);
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
            int group = WhatGroup(sender, cb);

            label1.Content = "Autoping " + (group + 1);

            //CheckChange(grid[group], timer[group], cb[group], (group + 1), g_settings[group, 0]);
        }

        private void PingClick(object sender, EventArgs e)
        {
            int group = WhatGroup(sender, ping);

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
            int group = WhatGroup(sender, stnx);

            label1.Content = "Settings " + (group + 1);

            //ShowSettings(group + 1);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            int group = WhatGroup(sender, timer);

            /*button0.Enabled = false;
            ping_completed[group] = false;

            ClearGrid(grid[group], g_settings[group, 0]);
            SortPing(group + 1);*/
        }

        private void GridCellClick(object sender, MouseEventArgs e)
        {
            int group = WhatGroup(sender, dtgr);

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

        private int WhatGroup(object sender, object[] compare_with)
        {
            int result;

            for (result = 0; result < tab_count; result++)
                if (sender.Equals(compare_with[result]))
                    break;

            return result;
        }

        private void ResizeForm()
        {
            int sizew, sizeh = (int)GridGroup.ActualHeight / 2;

            for (int i = 0; i < tab_count; i++)
            {
                int locx, locy = 0;

                if (i < first_half)
                {
                    sizew = (int)GridGroup.ActualWidth / first_half;
                    locx = i * ((int)GridGroup.ActualWidth / first_half);
                }
                else
                {
                    sizew = (int)GridGroup.ActualWidth / second_half;
                    locx = (i - first_half) * ((int)GridGroup.ActualWidth / second_half);
                    locy = sizeh;
                }

                grid[i].Margin = new Thickness(locx, locy, 0, 0);
                grid[i].Width = sizew - 4;
                grid[i].Height = sizeh - 2;
            }
        }

        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeForm();
        }
    }
}
