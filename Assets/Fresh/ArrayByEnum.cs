
using System.Collections;
using System;
using System.Linq;

public class ArrayByEnum<T, U> : IEnumerable where U : Enum {
  private readonly T[] _array;

  public ArrayByEnum(params T[] initial) {
    _array = initial;
  }

  public T this[U key] {
    get { return _array[Convert.ToInt32(key)]; }
    set { _array[Convert.ToInt32(key)] = value; }
  }

  public IEnumerator GetEnumerator() {
    return Enum.GetValues(typeof(U)).Cast<U>().Select(i => this[i]).GetEnumerator();
  }

  public void ForEach(Action<T, U> callback) {
    var data = Enum.GetValues(typeof(U)).Cast<U>().Select(i => (this[i], i));
    foreach ((T item, U index) in data) {
      callback(item, index);
    }
  }
}
