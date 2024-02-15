using System.CodeDom;
using System.DirectoryServices.ActiveDirectory;
using System.Threading.Channels;

namespace rps_sim
{
    public partial class Form1 : Form

    {
        public const int MAX_MOVEMENT = 10;
        public const int GAME_TICK = 100;// tenth of a second

        List<player> scissors = new List<player>();
        List<player> rock = new List<player>();
        List<player> paper = new List<player>();
        System.Windows.Forms.Timer MyTimer = new System.Windows.Forms.Timer();

        Random rand = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyTimer.Interval = (GAME_TICK); 
            MyTimer.Tick += new EventHandler(Timer_Tick);
            MyTimer.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int numS = (int)numericUpDown1.Value;
            int numR = (int)numericUpDown2.Value;
            int numP = (int)numericUpDown3.Value;

            Spawn(Option.Rock, numR, rock);
            Spawn(Option.Paper, numP, paper);
            Spawn(Option.Scissors, numS, scissors);




            MyTimer.Enabled = true;
            MyTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            


            List<player>[] arr = { rock, paper, scissors };

            
            
            for (int i = 0; i < arr.Length; i++)
            {
                foreach (player p in arr[i].ToArray())
                {
                    //0 for up, 1 for down, 2 for right , 3 for left
                    int direction = GetDirection(p);
                    if (direction == 0)
                    {
                        if (p.picture.Location.Y > (MAX_MOVEMENT+1))
                        {
                            p.picture.Location = new Point(p.picture.Location.X, p.picture.Location.Y - MAX_MOVEMENT);
                        }
                    }
                    else if (direction == 1)
                    {
                        if (p.picture.Location.Y < panel1.Size.Height - (MAX_MOVEMENT+1))
                        {
                            p.picture.Location = new Point(p.picture.Location.X, p.picture.Location.Y + MAX_MOVEMENT);
                        }
                    }
                    else if (direction == 2)
                    {
                        if (p.picture.Location.X < panel1.Size.Width - (MAX_MOVEMENT+1))
                        {
                            p.picture.Location = new Point(p.picture.Location.X + MAX_MOVEMENT, p.picture.Location.Y);
                        }
                    }
                    else if (direction == 3)
                    {
                        if (p.picture.Location.X > MAX_MOVEMENT+1)
                        {
                            p.picture.Location = new Point(p.picture.Location.X - MAX_MOVEMENT, p.picture.Location.Y);
                        }
                    }

                    if (i == 0)
                    {
                        foreach(player p_ in arr[2].ToArray())
                        {
                            if (p.picture.Bounds.IntersectsWith(p_.picture.Bounds))
                            {
                                p_.type = 1;
                                p_.picture.BackgroundImage = Image.FromFile("rock.png");
                                arr[i].Add(p_);
                                arr[2].Remove(p_);
                            }
                        }
                    } else if (i == 1)
                    {
                        foreach (player p_ in arr[0].ToArray())
                        {
                            if (p.picture.Bounds.IntersectsWith(p_.picture.Bounds))
                            {
                                p_.type = 2;
                                p_.picture.BackgroundImage = Image.FromFile("paper.png");
                                arr[i].Add(p_);
                                arr[0].Remove(p_);
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        foreach (player p_ in arr[1].ToArray())
                        {
                            if (p.picture.Bounds.IntersectsWith(p_.picture.Bounds))
                            {
                                p_.type = 3;
                                p_.picture.BackgroundImage = Image.FromFile("scissors.png");
                                arr[i].Add(p_);
                                arr[1].Remove(p_);
                            }
                        }
                    }
                }

            }

            if (arr[0].Count == 0 && arr[1].Count == 0)
            {
                Destroy();
                MessageBox.Show("Scissors wins");
                
                return;
            }
            else if (arr[1].Count == 0 && arr[2].Count == 0)
            {
                Destroy();
                MessageBox.Show("Rock wins");
                
                return;
            }
            else if (arr[0].Count == 0 && arr[2].Count == 0)
            {
                Destroy();
                MessageBox.Show("Paper wins");
                
                return;
            }
        }
        private void Destroy()
        {
            MyTimer.Stop();
            MyTimer.Enabled = false;
            List<player>[] arr = { rock, paper, scissors };
            foreach(List<player> item in arr)
            {
                foreach(player p in item)
                {
                    this.Controls.Remove(p.picture);
                }
            }
            
        }

        private int GetDirection(player p)
        {
            //0 for up, 1 for down, 2 for right , 3 for left
            int upWeight = 25;
            int downWeight = 25;
            int rightWeight = 25;
            int leftWeight = 25;
            if(p.picture.Location.X > panel1.Width/2)
            {
                //we want to move left
                rightWeight = 15;
                leftWeight = 35;
            } else if(p.picture.Location.X < panel1.Width/2)
            {
                rightWeight = 35;
                leftWeight = 15;
            }else if(p.picture.Location.Y > panel1.Height / 2)
            {
                //we want to move up
                upWeight = 35;
                downWeight = 15;
            }else if(p.picture.Location.Y < panel1.Height / 2)
            {
                upWeight = 15;
                downWeight = 35;
            }
            int[] weights = { upWeight, downWeight, rightWeight, leftWeight };
            int x = rand.Next(100);
            int iteration = 0; 
            foreach (int c in weights)
            {
                iteration++;
                if ((x -= c) < 0)
                    break;
            }

            switch (iteration)
            {
                case 1:
                    return 0;
                case 2:
                    return 1;
                case 3:
                    return 2;
                case 4:
                    return 3;
                //...
                default:
                    return rand.Next(4);
            }
        }

        private void Spawn(Option type, int num, List<player> player_)
        {
            if (type == Option.Rock)
            {
                for (int i = 0; i < num; i++)
                {
                    PictureBox picture_ = new PictureBox
                    {
                        Name = "Rock " + i.ToString(),
                        BackgroundImage = Image.FromFile("rock.png"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Size = new Size(21, 24),

                        Location = getSpawnLoc(type)
                    };
                    player_.Add(
                        new player { picture = picture_, type = (int)Option.Rock }
                        );
                    this.Controls.Add(picture_);
                    picture_.BringToFront();
                }
            }
            else if (type == Option.Paper)
            {
                for (int i = 0; i < num; i++)
                {
                    
                    PictureBox picture_ = new PictureBox
                    {
                        Name = "Paper " + i.ToString(),
                        BackgroundImage = Image.FromFile("paper.png"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Size = new Size(21, 24),

                        Location = getSpawnLoc(type)
                    };
                    player_.Add(
                        new player { picture = picture_, type = (int)Option.Paper }
                        );
                    this.Controls.Add(picture_);
                    picture_.BringToFront();
                }
            }
            else if (type == Option.Scissors)
            {
                for (int i = 0; i < num; i++)
                {
                    PictureBox picture_ = new PictureBox
                    {
                        Name = "Scissors " + i.ToString(),
                        BackgroundImage = Image.FromFile("scissors.png"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Size = new Size(21, 24),

                        Location = getSpawnLoc(type)
                    };
                    player_.Add(
                        new player { picture = picture_, type = (int)Option.Scissors }
                        );
                    this.Controls.Add(picture_);
                    picture_.BringToFront();
                }
            }
        }
        private Point getSpawnLoc(Option type)
        {
            Point loc;

            if (type == Option.Rock)
            {
                int x = rand.Next(RockBox.Location.X, RockBox.Location.X + RockBox.Width);
                int y = rand.Next(RockBox.Location.Y, RockBox.Location.Y + RockBox.Height);
                loc = new Point(x, y);
                return loc;
            }
            else if (type == Option.Paper)
            {
                int x = rand.Next(PaperBox.Location.X, PaperBox.Location.X + PaperBox.Width);
                int y = rand.Next(PaperBox.Location.Y, PaperBox.Location.Y + PaperBox.Height);
                loc = new Point(x, y);
                return loc;
            }
            else if (type == Option.Scissors)
            {
                int x = rand.Next(ScissorBox.Location.X, ScissorBox.Location.X + ScissorBox.Width);
                int y = rand.Next(ScissorBox.Location.Y, ScissorBox.Location.Y + ScissorBox.Height);
                loc = new Point(x, y);
                return loc;
            }
            return new Point(0, 0);

        }
        enum Option
        {
            None,
            Rock,
            Paper,
            Scissors
        }
    }

    public class player
    {
        public PictureBox picture { get; set; }
        public int type { get; set; }

    }
}