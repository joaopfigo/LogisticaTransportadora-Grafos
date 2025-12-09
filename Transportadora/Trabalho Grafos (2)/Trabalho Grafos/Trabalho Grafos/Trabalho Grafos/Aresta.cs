using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho_Grafos
{
    internal class Aresta
    {
        private int origem;
        private int destino;
        private double peso;
        private double capacidade;
        public Aresta(int origem, int destino, double peso, double capacidade)
        { 
            this.origem = origem;
            this.destino = destino;
            this.peso = peso;
            this.capacidade = capacidade;
        }
        public int getOrigem()
        {
            return origem;
        }
        public int getDestino() 
        {            
            return destino;
        }
        public double getPeso()
        {
            return peso;
        }
        public double getCapacidade()
        {
            return capacidade;
        }
        public void setCapacidade(double novaCapacidade)
        {
            capacidade = novaCapacidade;
        }
    }
}
