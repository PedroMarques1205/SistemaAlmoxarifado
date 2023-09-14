using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALMOXARIFADO.TI.Classes
{
    internal class Equipmentos
    {
        private int _Equipmento_ID;
        public int Equipmento_ID
        {
            get { return _Equipmento_ID; }
            set { _Equipmento_ID = value; }
        }

        private string _Equipmento_Nome;
        public string Equipmento_Nome
        {
            get { return _Equipmento_Nome; }
            set { _Equipmento_Nome = value; }
        }

        private int _Quantidade;
        public int Quantidade
        {
            get { return _Quantidade; }
            set { _Quantidade = value; }
        }

        public Equipmentos()
        {

        }

        public Equipmentos(int Equipmento_ID_, string Equipmento_Nome_, int Quantidade_)
        {
            this.Equipmento_ID = Equipmento_ID_;
            this.Equipmento_Nome = Equipmento_Nome_;
            this.Quantidade = Quantidade_;
        }
    }
}
