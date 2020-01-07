using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TowersOfHanoi.Views;

namespace TowersOfHanoi
{
    class Calculaton
    {
        //De methode
        public void Berekening(MainForm form)
        {
            ulong aantalSchijven;

            ulong.TryParse(form.textBox1.Text, out aantalSchijven);

            if(aantalSchijven == 69)
            {
                return;
            }

            if (!ulong.TryParse(form.textBox1.Text, out aantalSchijven))
            {
                MessageBox.Show("Geef het aantal schijven op in een GETAL!");

                return;
            }


            if(aantalSchijven > 64)
            {
                MessageBox.Show("Geef een getal onder de 64 AUB");

                return;
            }
            //Tijd voor berekening

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();

            //Stappen uitrekenen

            ulong aantalStappen = (ulong)Math.Pow(2, aantalSchijven) - 1;

            //Voor het uitrekenen

            ulong totaleTijdInSeconden = aantalStappen;

            ulong totaleTijdInMinuten = 60;

            ulong totaleTijdInUren = 3600;

            ulong totaleTijdInDagen = 86400;

            ulong totaleTijdInWeken = 604800;

            ulong totaleTijdInJaren = 31536000;

            ulong totaleTijdInDecenia = 315360000;

            ulong totaleTijdInEeuwen = 3153600000;

            ulong overigeAantalSeconden = 0;

            //Het daadwerkelijke uitrekenen

            ulong eeuwFinal = (ulong)totaleTijdInSeconden / totaleTijdInEeuwen;

            overigeAantalSeconden = totaleTijdInSeconden % totaleTijdInEeuwen;

            ulong deceniaFinal = overigeAantalSeconden / totaleTijdInDecenia;

            overigeAantalSeconden = overigeAantalSeconden % totaleTijdInDecenia;

            ulong jaarFinal = overigeAantalSeconden / totaleTijdInJaren;

            overigeAantalSeconden = overigeAantalSeconden % totaleTijdInJaren;

            ulong weekFinal = overigeAantalSeconden / totaleTijdInWeken;

            overigeAantalSeconden = overigeAantalSeconden % totaleTijdInWeken;

            ulong dagFinal = overigeAantalSeconden / totaleTijdInDagen;

            overigeAantalSeconden = overigeAantalSeconden % totaleTijdInDagen;

            ulong uurFinal = overigeAantalSeconden / totaleTijdInUren;

            overigeAantalSeconden = overigeAantalSeconden % totaleTijdInUren;

            ulong minuutFinal = overigeAantalSeconden / totaleTijdInMinuten;

            overigeAantalSeconden = overigeAantalSeconden % totaleTijdInMinuten;

            //Tijd stoppen

            watch.Stop();

            //Tijd laten zien

            form.timeCalculationLabel.Text = "De berekening duurt " + watch.Elapsed.TotalMilliseconds + " ms om uit te rekenen";

            //Conclusie

            form.movesLabel.Text = "Je hebt " + aantalStappen + " stappen nodig om de Torens van Hanoi\n" +
                "met " + aantalSchijven + " schijven op te lossen";

            form.centuryResult.Text = eeuwFinal + " Eeuw";

            form.deceniaResult.Text = deceniaFinal + " Decenia";

            form.yearResult.Text = jaarFinal + " Jaar";

            form.weekResult.Text = weekFinal + " Weken";

            form.dayResult.Text = dagFinal + " Dagen";

            form.hourResult.Text = uurFinal + " Uren";

            form.minuteResult.Text = minuutFinal + " Minuten";

            form.SecondResult.Text = overigeAantalSeconden + " Seconden";
        }
    }
}
