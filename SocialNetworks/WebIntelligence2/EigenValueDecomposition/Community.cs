using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace SocialMediaAnalysis
{
    class Community
    {
        private readonly Guid _id;

        public List<User> Users { get; private set; }

        public Guid Id
        {
            get { return _id; }
        }

        private Matrix<double> _adjacencyMatrix;

        public int Size
        {
            get { return Users.Count; }
        }

        public Community()
        {
            _id = Guid.NewGuid();
            Users = new List<User>();
        }

        public void AddUser(User user)
        {
            user.CommunityId = _id;
            Users.Add(user);
        }

        public User GetUser(string name)
        {
            return Users.FirstOrDefault(x => string.Equals(x.Name, name));
        }

        public Matrix<double> AdjacencyMatrix
        {
            get
            {
                if (_adjacencyMatrix == null)
                    _adjacencyMatrix = GetAdjacencyMatrix();
                return _adjacencyMatrix;
            }
        }

        private Matrix<double> GetAdjacencyMatrix()
        {
            Matrix<double> matrix = Matrix<double>.Build.Dense(Users.Count, Users.Count, 0);

            for (int i = 0; i < Users.Count; i++)
            {
                User user = Users[i];
                string name = user.Name;
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

        public string ToGraph()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Users.Count; i++)
            {
                User user = Users[i];
                string name = user.Name;
                for (int j = 0; j < Users.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (Users[j].HasFriend(name))
                    {
                        sb.AppendLine(user.Name + "," + Users[j].Name);
                    }
                }
            }
            return sb.ToString();
        }
    }
}
