using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace SocialMediaAnalysis
{
    class Community : IDisposable
    {
        private readonly Guid _id;
        private List<User> _users; 

        public List<User> Users
        {
            get
            {
                foreach (var user in _users)
                {
                    user.CommunityId = _id;
                }
                return _users;
            }
        }

        public Guid Id
        {
            get { return _id; }
        }

        private Matrix<double> _adjacencyMatrix;

        public int Size
        {
            get { return _users.Count; }
        }

        public Community()
        {
            _id = Guid.NewGuid();
            _users = new List<User>();
        }

        public void AddUser(User user)
        {
            user.CommunityId = _id;
            _users.Add(user);
        }

        public User GetUser(string name)
        {
            return _users.FirstOrDefault(x => string.Equals(x.Name, name));
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
            Matrix<double> matrix = Matrix<double>.Build.Dense(_users.Count, _users.Count, 0);

            for (int i = 0; i < _users.Count; i++)
            {
                User user = _users[i];
                string name = user.Name;
                for (int j = 0; j < _users.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (_users[j].HasFriend(name))
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
            for (int i = 0; i < _users.Count; i++)
            {
                User user = _users[i];
                string name = user.Name;
                for (int j = 0; j < _users.Count; j++)
                {
                    if (i == j)
                        continue;
                    if (_users[j].HasFriend(name))
                    {
                        sb.AppendLine(user.Name + "," + _users[j].Name);
                    }
                }
            }
            return sb.ToString();
        }

        public void Dispose()
        {
            _adjacencyMatrix = null;
            _users = null;
        }
    }
}
