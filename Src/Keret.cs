﻿using System;
using OE.Prog2.Jatek.Jatekter;
using OE.Prog2.Jatek.Szabalyok;

namespace OE.Prog2.Jatek.Keret
{
    class Keret
    {
        private const int PALYA_MERET_X = 21; //a pálya maximális mérete x irányba (21)
        private const int PALYA_MERET_Y = 11; //a pálya maximális mérete y irányba (11)
        private const int KINCSEK_SZAMA = 10; //a pályán lévő kincsek száma (10)
        private JatekTer ter; //pályatér 
        private Random R = new Random(); //véletlen szám kincsekhez
        private JatekElem[] segedelem; //segéd tömb, különböző véletlen kincs pozíciókhoz
        private bool jatekVege = false; //csak akkor lesz igaz, ha vége a játéknak

        private void PalyaGeneralas()
        {
            for (int i = 0; i < PALYA_MERET_X; i++) //vízszintes falak
            {
                new Fal(i, 0, ref ter);
                new Fal(i, PALYA_MERET_Y, ref ter);
            }
            
            for (int i = 0; i < PALYA_MERET_Y; i++) //függőleges falak
            {
                new Fal(0, i, ref ter);
                new Fal(PALYA_MERET_X, i, ref ter);
            }

            //Véletlenszerűen szórjon szét KINCSEK_SZAMA darab kincset a pályán belül, ügyelve arra,
            //hogy egy helyre nem fér el egynél több Kincs objektum
            //(az 1,1 pozíciót is hagyjuk szabadon, innen indul majd a játékos).
            int db = 0;
            int ujx, ujy;
            bool jatekos;
            do
            {
                ujx = R.Next(PALYA_MERET_X + 1);
                ujy = R.Next(PALYA_MERET_Y + 1);
                segedelem = ter.MegadottHelyenLevok(ujx, ujy);
                jatekos = ujx == 1 && ujy == 1;
                if (segedelem.Length == 0 && !jatekos)
                {
                    new Kincs(ujx, ujy, ref ter);
                    db++;
                }
            }
            while (db != (KINCSEK_SZAMA - 1));
        }

        //hozza létre a PalyaTer objektumot a maximális mérettel, majd hívja meg a PalyaGeneralas metódust
        public Keret()
        {
            ter = new JatekTer(PALYA_MERET_X, PALYA_MERET_Y);
            PalyaGeneralas();
        }

        //Létrehoz egy játékost „Béla” néven az 1,1 pozícióba a játéktérben
        //Egy ciklus addig fusson, amíg a jatekVege metódus nem vált igazra.
        //A cikluson belül kérjen be egy billentyűleütést, és ha ez egy kurzor gomb volt,
        //akkor mozgassa Bélát ebbe az irányba, ha az Esc billentyű, akkor pedig lépjen ki.
        public void Futtatas()
        {
            Jatekos jatekos = new Jatekos("Béla", 1, 1, ref ter);
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.LeftArrow) jatekos.Megy(-1, 0);
                if (key.Key == ConsoleKey.RightArrow) jatekos.Megy(1, 0);
                if (key.Key == ConsoleKey.UpArrow) jatekos.Megy(0, -1);
                if (key.Key == ConsoleKey.DownArrow) jatekos.Megy(0, 1);
                if (key.Key == ConsoleKey.Escape) jatekVege = true;
            } while (!jatekVege);
        }
    }
}