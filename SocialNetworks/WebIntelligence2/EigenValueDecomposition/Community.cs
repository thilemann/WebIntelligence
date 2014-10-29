using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;

namespace EigenValueDecomposition
{
    class Community
    {
        public List<User> Users { get; private set; }
        private double[,] _adjacencyMatrix;

        public Community()
        {
            Users = new List<User>();
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public double[,] AdjacencyMatrix
        {
            get
            {
                if (_adjacencyMatrix == null)
                    _adjacencyMatrix = GetAdjacencyMatrix();
                return _adjacencyMatrix;
            }
        }
        
        private double[,] GetAdjacencyMatrix()
        {
            double[,] matrix = Matrix.Create<double>(Users.Count, Users.Count, 0);

            for (int i = 0; i < Users.Count; i++)
            {
                User user = Users[i];
                string name = user.Name.ToLower();
                for (int j = 0; j < Users.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (Users[j].HasFriend(name))
                    {
                        matrix[i, j] = 1;
                    }
                }
            }
            return matrix;
        }
    }
}
