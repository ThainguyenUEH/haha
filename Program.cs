
using System.IO;
using System.Text;

namespace Game
{
    class Display
    {
        public static int
            oldkhunglong = 0, // Vị trí trước đó của khủng long, dùng để xoá vết in của khủng long khi nhảy lên hay rớt xuống
            klongpicture = 0, // Số lượng hình dùng làm hình động cho khủng long
            dodai = 0,        // Độ dài của hình con đường
            chieucaokl = 0,   // Chiều cao khủng long, chieucaokl = số hàng của khunglong[0], dùng để kiểm tra va chạm
            chieurongkl = 0;  // Chiều rộng khủng long, chieurongkl = chiều dài của hàng dài nhất trong khunglong[0], dùng để kiểm tra va chạm

        public static List<int> oldxuongrong = new List<int>(); // Vị trí trước đó của các cây xương rồng mọc trên đường, dùng để xoá vết in khi các cây xương rồng dịch chuyển
        public static void Khunglong(int nhay, int chay)
        {
            // Vị trí (x,y) của khủng long trên mặt đất
            int x = 15,
                y = Console.BufferHeight / 2 + 2;

            string[][] khunglong = new string[3][];
            khunglong[0] = new string[]
            {
                "        ▄▄▄▄",
                "        █▄███",
                "█▄    ▄████ ",
                " ██▄▄▄████▄",
                "  ▀██████▀ ▀",
                "     █ █▄"
            };
            khunglong[1] = new string[]
            {
                "        ▄▄▄▄",
                "        █▄███",
                "█▄    ▄████ ",
                " ██▄▄▄████▄",
                "  ▀██████▀ ▀",
                "     ▀ █▄"
            };
            khunglong[2] = new string[]
            {
                "        ▄▄▄▄",
                "        █▄███",
                "█▄    ▄████ ",
                " ██▄▄▄████▄",
                "  ▀██████▀ ▀",
                "     █ ▀▀"
            };

            // Gán giá trị cho klongpicture, chieucaokl, chieurongkl
            klongpicture = khunglong.Length;
            chieucaokl = khunglong[0].Length;
            for (int i = 0; i < khunglong[0].Length; i++)
                if (chieurongkl < khunglong[0][i].Length) chieurongkl = khunglong[0][i].Length;

            // Với nhay == 0 là khủng long còn trên mặt đất, in ra màn hình khủng long đang chạy
            if (nhay == 0)
            {
                // Xoá vết in cũ của khủng long
                for (int i = 0; i < khunglong[chay].Length; i++)
                    for (int j = 0; j < khunglong[chay][i].Length; j++)
                    {
                        if (khunglong[chay][i][j] != ' ') Print(x + j, y + i - 1, " ");
                    }
                // In ra khủng long
                for (int i = 0; i < khunglong[chay].Length; i++)
                    for (int j = 0; j < khunglong[chay][i].Length; j++)
                    {
                        if (khunglong[chay][i][j] != ' ') Print(x + j, y + i, khunglong[chay][i][j].ToString());
                    }
            }

            // nhay != 0 khủng long nhảy lên
            else
            {
                // Xoá vết in cũ
                for (int i = 0; i < khunglong[0].Length; i++)
                    for (int j = 0; j < khunglong[0][i].Length; j++)
                    {
                        if (khunglong[0][i][j] != ' ') Print(x + j, y + i - oldkhunglong, " ");
                    }
                // In ra
                for (int i = 0; i < khunglong[0].Length; i++)
                    for (int j = 0; j < khunglong[0][i].Length; j++)
                        if (khunglong[0][i][j] != ' ') Print(x + j, y + i - nhay, khunglong[0][i][j].ToString());
            }

            oldkhunglong = nhay;
        }
        public static void Xuongrong(int khoidau, byte loai, bool spawn) // 'loai' loại cây xương rồng, tạo thêm đa dạng cho cây xương rồng (hiện chỉ có 1 loại)
        {
            // Vị trí của xương rồng
            int x = 0,
                y = Console.BufferHeight / 2 + 3;

            string[][] xuongrong = new string[1][];
            xuongrong[0] = new string[]
            {
                "  ▄▄",
                "█ ██",
                "█▄██ █",
                "  ██▀▀",
                "  ██"
            };

            // spawn == true thì thêm một cây xương rồng mới trên đường chạy
            if (spawn) oldxuongrong.Add(khoidau);

            // spawn == false thì các xương rồng đã xuất hiện trước đó vẫn dịch chuyển theo đường chạy của khủng long
            else
            {
                int k; // Gán k = vị trí của cây xương rồng thứ h đang xuất hiện trên đường chạy
                for (int h = 0; h < oldxuongrong.Count; h++)
                {
                    k = oldxuongrong[h];
                    int j0 = 0; // Vị trí bắt đầu để in ra các kí tự trong xuongrong[loai][i], xương rồng chuẩn bị biến mất
                    int jn;     // Vị trí cuối cùng để in ra các kí tự trong xuongrong[loai][i], xương rồng chuẩn bị xuất hiện

                    Clear(1, Console.BufferHeight / 2 + 3, 1, 5); // Xoá vết in cây xương rồng khi biến mất, (cần góp ý)
                    for (int i = 0; i < xuongrong[loai].Length; i++)
                    {
                        /*
                         'k' vị trí xương rồng, dùng để dịch chuyển cây xương rồng. 
                        'k' có thể bé hơn 0, nếu dùng 'k' để in luôn thì không thể in ra màn hình ở một số giá trị.
                         */

                        // Xử lý trường hợp 'k' có giá trị làm cho việc in ra cây xương rồng nằm ngoài bên trái Bufferwidth
                        if (k <= 1 && Math.Abs(1 - k) < xuongrong[loai][i].Length) // Số '1', tại vì mình muốn bỏ đi 2 cột cạnh trái, cạnh phải của màn hình, để in ra nội dung game không bị dính sát vào khung màn hình
                            j0 = Math.Abs(1 - k);

                        // Xử lý trường hợp 'k' có giá trị làm cho việc in ra cây xương rồng nằm ngoài bên phải Bufferwidth
                        if (Console.BufferWidth - 1 - k < xuongrong[loai][i].Length)
                        {
                            jn = Math.Abs(Console.BufferWidth - 1 - k);
                        }
                        else
                            jn = xuongrong[loai][i].Length;

                        // Xoá vết in cũ
                        for (int j = j0; j < jn; j++)
                            if (x + j + k > 0) Print(x + j + k + 1, y + i, " ");
                    }

                    for (int i = 0; i < xuongrong[loai].Length; i++)
                    {
                        if (k <= 1 && Math.Abs(1 - k) < xuongrong[loai][i].Length) // Bị lặp, cần cải tiến
                            j0 = Math.Abs(1 - k);
                        if (Console.BufferWidth - 1 - k < xuongrong[loai][i].Length)
                        {
                            jn = Math.Abs(Console.BufferWidth - 1 - k);
                        }
                        else
                            jn = xuongrong[loai][i].Length;

                        // In ra 
                        for (int j = j0; j < jn; j++)
                            if (xuongrong[loai][i][j] != ' ' && x + j + k > 0)
                                Print(x + j + k, y + i, xuongrong[loai][i][j].ToString());
                    }

                    oldxuongrong[h]--;

                    // Xoá cây xương rồng đã chạy qua
                    if (oldxuongrong[h] == -10) oldxuongrong.RemoveAt(h);
                }
            }
        }
        public static void Conduong(int dichchuyen)
        {
            // Vị trí của con đường
            int x = 1,
                y = Console.BufferHeight / 2 + 3;

            string[] conduong = new string[]
            {
                "▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄   ▄▄▄▄▄▄▄▄▄▀▀▀█▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄",
                "     ▀  ▄▄       ▄▄           ▀▀▀                  ▄▄               ",
                "▀▀                                   █                              "
            };
            dodai = conduong[0].Length;

            // In ra con đường
            for (int i = 0; i < conduong.Length; i++)
            {
                int k = dichchuyen;
                for (int j = 0; j < Console.BufferWidth - 1; j++)
                {
                    if (k >= conduong[0].Length) k = 0;
                    Print(x + j, 5 + y + i, conduong[i][k].ToString());
                    k++;
                }
            }
        }
        public static void Batdau()
        {
            string[] batdau = new string[]
            {
                "┏━━━━━━━┓",
                "┃ Space ┃",
                "┗━━━━━━━┛"
            };
            for (int i = 0; i < batdau.Length; i++)
                Print((Console.BufferWidth - batdau[0].Length) / 2, (Console.BufferHeight - batdau.Length) / 2 + i, batdau[i]);
        }
        public static void Gameover()
        {
            string[] gameover = new string[]
            {
                "G A M E  O V E R !",
                "    ┏━━━━━━━┓     ",
                "    ┃ Enter ┃     ",
                "    ┗━━━━━━━┛     ",
                "   Lưu kết quả    "
            };
            for (int i = 0; i < gameover.Length; i++)
                Print((Console.BufferWidth - gameover[0].Length) / 2, (Console.BufferHeight - gameover.Length) / 2 + i, gameover[i]);
        }
        public static void Luukq()
        {
            string[] luukq = new string[]
            {
                "┏━━━━━━━━━━━━━━━━━━━━━━━┓",
                "┃ Tên:                  ┃",
                "┗━━━━━━━━━━━━━━━━━━━━━━━┛",
            };
        }
        public static void Bangxephang(Dictionary<string, int> Diemso, byte hang)
        {
            byte x = 4, y = 2;
            string[] bangxephang = new string[]
            {
                "                     BẢNG XẾP HẠNG                    ",
                "┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓",
                "┃                                                  ▲ ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                    ┃",
                "┃                                                  ▼ ┃",
                "┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛",
            };
            int vt1 = (Console.BufferWidth - bangxephang[0].Length) / 2,
                vt2 = (Console.BufferHeight - bangxephang.Length) / 2;

            for (int i = 0; i < bangxephang.Length; i++)
                Print(vt1, vt2 + i, bangxephang[i]);

            Clear(vt1 + x, vt2 + y, bangxephang[0].Length - 2 * x, bangxephang.Length - y - 1);
            for (int i = 0; i < Diemso.LongCount(); i++) ;
        }

        // "Khoá" chỉ cho một luồng truy cập, đợi đến khi luồng đó truy cập xong thì luồng khác mới được truy cập
        private static readonly object locker = new object();
        public static void Print(int x, int y, string s)
        // In ra màn hình chuỗi s với vị trí (x,y)
        {
            lock (locker) // Sử dụng khoá luồng
            {
                Console.SetCursorPosition(x, y);
                Console.Write(s);
            }
        }
        public static void Clear(int x, int y, int w, int h)
        // Xoá một vùng trên màn hình bắt đầu với vị trí (x,y), chiều rộng w, chiều cao h
        {
            string c = "";

            for (int i = 0; i < w; i++)
                c += " ";

            for (int i = 0; i < h; i++)
                Print(x, y + i, c);
        }
    }
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;

            ConsoleKeyInfo key = new ConsoleKeyInfo(); // Nhận tương tác bằng các phím của người dùng
            int maxjump = 15,     // Độ cao nhảy cao nhất
                nhay = 0,         // Độ cao khi đang nhảy
                spawncactus = 0,  // Dùng để Random xuất hiện cây xương rồng
                dichchuyen = 0,   // Dùng để dịch chuyển con đường
                tocdo = 800;      // Tốc độ địch chuyển của con đường
            int[,] ktvacham = new int[maxjump + Display.chieucaokl, Display.chieurongkl + 2]; // Dùng để kiểm tra va chạm

            Display.Print(0, 0, "<Esc> thoát");
            Display.Print(15, 0, "<SPACEBAR> nhảy");

            // Khủng long chạy
            Thread run = new Thread(() =>
            {
                int i = 0;
                while (key.Key != ConsoleKey.Escape)
                {
                    if (i == Display.klongpicture) i = 0;
                    if (nhay == 0)
                    {
 
                        Display.Khunglong(0, i);
                        Thread running = new Thread(() =>
                        {
                            Thread.Sleep(1000 / tocdo);
                        });
                        running.Start();
                        running.Join();
                        i++;
                    }
                }
            });
            run.Start();

            // Cảnh vật dịch chuyển
            Thread canhvat = new Thread(() =>
            {
                Random vatcan = new Random();
                while (key.Key != ConsoleKey.Escape)
                {
                    if (dichchuyen >= Display.dodai - 1) dichchuyen = 0;

                    Display.Conduong(dichchuyen);
                    Thread.Sleep(1000 / tocdo);

                    if (vatcan.Next(1, 25) == 1 && (spawncactus > 40 || (spawncactus < 6 && spawncactus > 3)))
                    // Tỉ lệ xuất hiện 1/25, tránh trường hợp các cây xương rồng xuất hiện nhiều và gần nhau
                    {
                        spawncactus = 0;
                        Display.Xuongrong(Console.BufferWidth - 1, 0, true);
                    }

                    Display.Xuongrong(0, 0, false);

                    dichchuyen++;
                    spawncactus++;
                }
            });
            canhvat.Start();

            // In ra điểm số
            Thread diemso = new Thread(() =>
            {
                int diem = 0;
                while (key.Key != ConsoleKey.Escape)
                {
                    Display.Print(50, 0, "Điểm: " + diem);
                    Thread.Sleep(8000 / tocdo);
                    diem++;
                }
            });
            diemso.Start();

            // Xử lý phím mà người dùng nhấn vào
            while (true)
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape: Environment.Exit(0); break;
                    case ConsoleKey.Spacebar:
                        // Khủng long nhảy
                        Thread jump = new Thread(() =>
                        {
                            // Nhảy lên
                            for (int j = 0; j < maxjump; j++)
                            {
                                nhay = j;
                                Display.Khunglong(j, 0);
                                Thread.Sleep(30 + nhay / 2);
                            }
                            // Rớt xuống
                            for (int j = maxjump; j >= 0; j--)
                            {
                                nhay = j;
                                Display.Khunglong(j, 0);
                                Thread.Sleep(40 + nhay / 2);
                            }
                        });
                        jump.Start();
                        Console.Beep(500, 200);
                        jump.Join();
                        break;
                }
            }
        }
    }
}
