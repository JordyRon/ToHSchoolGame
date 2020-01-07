using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reactive.Disposables;

namespace Torens_van_Hanoi
{
    public partial class MainForm : Form
    {
        //Kleur dingen
        private static readonly CompositeDisposable Subscriptions = new CompositeDisposable();
        private readonly Color[] _colors =
        {
            Color.Aquamarine,
            Color.Red,
            Color.Purple,
            Color.Blue,
            Color.Lime
        };

        public MainForm()
        {
            InitializeComponent();
            var engine = GameEngine.Instance;
            for (var i = 0; i < _colors.Length; i++)
            {
                engine.AddDisc(0, i + 1, _colors[i]);
            }

            textBoxMoves.Text = engine.Moves.ToString();
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(engine, new ValidationContext(engine), results))
            {
                textBoxMessages.Text = results.Aggregate("", (a, v) => $"{a}{Environment.NewLine}{v.ErrorMessage}".Trim());
            }

            engine.ValidationAdded
                .Subscribe(v => textBoxMessages.Text = v.ErrorMessage)
                .DisposeWith(Subscriptions);

            engine.DiscMoved
                .Subscribe(_ =>
                {
                    if (engine.HasWon())
                    {
                        MessageBox.Show("Gewonnen!!!", "Info", MessageBoxButtons.OK);
                        engine.Reset();
                        textBoxMessages.Text = "";
                    }

                    textBoxMoves.Text = engine.Moves.ToString();
                })
                .DisposeWith(Subscriptions);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Easter egg

            ulong easter;

            ulong.TryParse(textBox1.Text, out easter);

            if (easter == 69)
            {
                easterEgg.Visible = true;
            }

            //Methode aangemaakt voor het berekenen

            Calculaton calculation = new Calculaton();

            calculation.Berekening(this);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            //Op de knop later drukken als je op enter drukt

            if(e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tohGame.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            easterEgg.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tohGame.Visible = false;
            easterEgg.Visible = false;
        }

        //ToH Game/Easter Egg
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
                Subscriptions.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
