using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VennBot
{
    public static class VennSolver
    {
        public static IEnumerable<int> Solve(string formula, IEnumerable<int> omega, Dictionary<char, IEnumerable<int>> sets) {

            int i = 0;
            IEnumerable<int> prev = GetNextSet(formula, omega, sets, ref i);

            while (i < formula.Length) {
                var op = GetNextOperator(formula, omega, sets, ref i);
                var succ = GetNextSet(formula, omega, sets, ref i);
                prev = prev.Operate<int>(succ, op);
            }
            return prev;

        }

        private static EOperator GetNextOperator(string formula, IEnumerable<int> omega, Dictionary<char, IEnumerable<int>> sets, ref int i) {
            EOperator res;
            if (formula[i] == '+')
                res = EOperator.Union;
            else if (formula[i] == '-')
                res = EOperator.Intersect;
            else if (formula[i] == '*')
                res = EOperator.Triangolo;
            else
                res = EOperator.OhNo;
            i++;
            return res;
        }

        private static IEnumerable<int> GetNextSet(string formula, IEnumerable<int> omega, Dictionary<char, IEnumerable<int>> sets, ref int i) {
            IEnumerable<int> res;
            if (formula[i] == '(') {
                res = Solve(BracketsSubFormula(formula, ref i), omega, sets);
            } else if (formula[i] == '0') {
                res = omega;
            } else {
                res = sets[formula[i]];
            }

            if ((i + 1 < formula.Length) && formula[i + 1] == '^') {
                i++;
                res = res.Complementare<int>(omega);
            }

            i++;
            return res;

        }

        private static string BracketsSubFormula(string formula, ref int i) {

            int start = i;
            int depth = 0;
            do {
                if (formula[i] == '(')
                    depth++;
                else if (formula[i] == ')')
                    depth--;
                i++;
            } while (depth != 0);
            i--;
            return formula.Substring(start + 1, i - start - 1);
        } 

        private enum EOperator
        {
            Union, Intersect, Triangolo, OhNo
        }


        private static IEnumerable<int> Complementare<T>(this IEnumerable<int> set, IEnumerable<int> omega) => omega.Where(n1 => !set.Any(n2 => n1 == n2));
        private static IEnumerable<int> Operate<T>(this IEnumerable<int> prev, IEnumerable<int> succ, EOperator op) {
            switch (op) {
                case EOperator.Union:
                    return prev.Union(succ);
                case EOperator.Intersect:
                    return prev.Intersect(succ);
                case EOperator.Triangolo:
                    return prev.Intersect(succ).Complementare<int>(prev.Union(succ));
                case EOperator.OhNo:
                default:
                    return new List<int>();
            }
        }
    }
}
