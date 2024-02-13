namespace rps_sim
{
    public partial class Form1 : Form

    {

        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int numS = (int)numericUpDown1.Value;
            int numR = (int)numericUpDown2.Value;
            int numP = (int)numericUpDown3.Value;
            List<player> scissors = new List<player>();
            List<player> rock = new List<player>();
            List<player> paper = new List<player>();
            Spawn(Option.Rock, numR, rock);
        }

        private void Spawn(Option type, int num, List<player> player_)
        {
            if(type== Option.Rock)
            {
                for (int i = 0; i < num; i++)
                {
                    PictureBox picture_ = new PictureBox
                    {
                        BackgroundImage = Image.FromFile("rock.png"),
                        BackgroundImageLayout = ImageLayout.Stretch,
                        Size = new Size(21, 24),
                        Location = getSpawnLoc(type)
                    };
                    player_.Add(
                        new player { picture = picture_ , type = (int)Option.Rock}
                        );
                    RockBox.Controls.Add( picture_ );
                }
            }
        }
        private Point getSpawnLoc(Option type)
        {
            Point loc;
            Random rand = new Random();
            if(type== Option.Rock)
            {
                int x = rand.Next(RockBox.Location.X, RockBox.Location.X+RockBox.Width);
                int y = rand.Next(RockBox.Location.Y, RockBox.Location.Y+RockBox.Height);
                loc = new Point(x, y);
                return loc;
            }else if (type == Option.Paper)
            {
                int x = rand.Next(PaperBox.Location.X, PaperBox.Location.X + PaperBox.Width);
                int y = rand.Next(PaperBox.Location.Y, PaperBox.Location.Y + PaperBox.Height);
                loc = new Point(x, y);
                return loc;
            } else if (type == Option.Scissors)
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
        public  int type { get; set; }
        
    }
}