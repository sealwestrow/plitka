using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PlitkaApp
{
    internal class SelectionGroup
    {
        public List<Polygon> Items { get; private set; } = new List<Polygon>();

        public void Add(Polygon p)
        {
            if (!Items.Contains(p))
            {
                Items.Add(p);
            }
        }

        public void Clear()
        {
            Items.Clear();
        }

        public bool Contains(Polygon p)
        {
            return Items.Contains(p);
        }

        public double GetLeftPoint()
        {
            var point = double.MaxValue;
            foreach (var poly in Items)
            {
                foreach (var p in poly.Points)
                {
                    if (p.X < point)
                        point = p.X;
                }
            }
            return point;
        }

        public double GetRightPoint()
        {
            var point = double.MinValue;
            foreach (var poly in Items)
            {
                foreach (var p in poly.Points)
                {
                    if (p.X > point)
                        point = p.X;
                }
            }
            return point;
        }

        public double GetTopPoint()
        {
            var point = double.MaxValue;
            foreach (var poly in Items)
            {
                foreach (var p in poly.Points)
                {
                    if (p.Y < point)
                        point = p.Y;
                }
            }
            return point;
        }

        public double GetBottomPoint()
        {
            var point = double.MinValue;
            foreach (var poly in Items)
            {
                foreach (var p in poly.Points)
                {
                    if (p.Y > point)
                        point = p.Y;
                }
            }
            return point;
        }
    }
}
