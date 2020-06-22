using System;
using System.Collections;
using System.Globalization;

namespace MSMQ
{
  [Serializable]
  internal class InvariantComparer : IComparer
  {
    internal static readonly InvariantComparer Default = new InvariantComparer();
    private CompareInfo m_compareInfo;

    internal InvariantComparer()
    {
      this.m_compareInfo = CultureInfo.InvariantCulture.CompareInfo;
    }

    public int Compare(object a, object b)
    {
        if (a is string string1 && b is string string2)
            return this.m_compareInfo.Compare(string1, string2);
        return Comparer.Default.Compare(a, b);
    }
  }
}
