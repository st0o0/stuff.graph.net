using System;
using System.Collections.Concurrent;

namespace stuff.graph.algorithms.net;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> _elements = [];

    public int Count => _elements.Count;

    public void Enqueue(T item)
    {
        _elements.Add(item);
        HeapifyUp(_elements.Count - 1);
    }

    public T Dequeue()
    {
        if (_elements.Count == 0)
            throw new InvalidOperationException("Priority queue is empty.");

        var item = _elements[0];
        _elements[0] = _elements[^1];
        _elements.RemoveAt(_elements.Count - 1);
        HeapifyDown(0);

        return item;
    }

    public bool Contains(T item) => _elements.Contains(item); 

    public void UpdatePriority(T item, double newPriority)
    {
        var index = _elements.IndexOf(item);
        if (index == -1)
            return;

        _elements[index] = item;
        HeapifyUp(index);
        HeapifyDown(index);
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;
            if (_elements[index].CompareTo(_elements[parentIndex]) >= 0)
                break;

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        while (index * 2 + 1 < _elements.Count)
        {
            var smallest = index;
            var left = index * 2 + 1;
            var right = index * 2 + 2;

            if (left < _elements.Count && _elements[left].CompareTo(_elements[smallest]) < 0)
                smallest = left;

            if (right < _elements.Count && _elements[right].CompareTo(_elements[smallest]) < 0)
                smallest = right;

            if (smallest == index)
                break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        (_elements[j], _elements[i]) = (_elements[i], _elements[j]);
    }
}