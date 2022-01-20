using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TD.OpenData.WebApi.Infrastructure.FileStorage.Models;

public class MetadataCollection : ICollection<Metadata>
{
    private readonly List<Metadata> _inner;

    public MetadataCollection()
    {
        _inner = new List<Metadata>();
    }

    public MetadataCollection(List<Metadata> inner)
    {
        _inner = inner;
    }

    public int Count => _inner.Count;

    public bool IsReadOnly => false;

    public void Add(Metadata item)
    {
        _inner.Add(item);
    }

    public void Clear()
    {
        _inner.Clear();
    }

    public bool Contains(Metadata item)
    {
        return _inner.Contains(item);
    }

    public void CopyTo(Metadata[] array, int arrayIndex)
    {
        _inner.CopyTo(array, arrayIndex);
    }

    public IEnumerator<Metadata> GetEnumerator()
    {
        return ((IEnumerable<Metadata>)_inner).GetEnumerator();
    }

    public bool Remove(Metadata item)
    {
        return _inner.Remove(item);
    }

    public override string ToString()
    {
        return string.Join("\n", _inner);
    }

    public string ToString(string separator)
    {
        return string.Join(separator, _inner);
    }

    public static MetadataCollection Parse(string data)
    {
        var res = new List<Metadata>();

        if (!string.IsNullOrEmpty(data))
        {
            var files = new Regex(@"\s*[\n#;]\s*")
                .Split(data)
                .Where(_ => !string.IsNullOrEmpty(data));

            foreach (string? str in files)
            {
                var f = Metadata.Parse(str);
                if (f != null)
                {
                    res.Add(f);
                }
            }
        }

        return new MetadataCollection(res);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_inner).GetEnumerator();
    }
}
