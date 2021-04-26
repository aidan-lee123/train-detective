using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Can be used for things that aren't just nodes but we know the max size
public class Heap<T> where T : IHeapItem<T> {

    T[] items;
    int currentItemCount;

    //Heap Constructor
    public Heap(int maxHeapSize) {
        items = new T[maxHeapSize];
    }

    //Add the item to the heap
    public void Add(T item) {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    //To remove from heap move it to the front and then sort down
    public T RemoveFirst() {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item) {
        SortUp(item);
    }

    //Accesor for count
    public int Count {
        get {
            return currentItemCount;
        }
    }

    public bool Contains(T item) {
        return Equals(items[item.HeapIndex], item);
    }

    //Check if parent has children and then compare the children to parent and move down
    void SortDown(T item) {
        while (true) {
            //To get the index of a child on the left it is 2n+1
            int childIndexLeft = item.HeapIndex * 2 + 1;
            //To get the index of a child on the right it is 2n+2
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            //Swap index is =  to child with highest priority
            if (childIndexLeft < currentItemCount) {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount) {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0) {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(items[swapIndex]) < 0) {
                    Swap(item, items[swapIndex]);
                }
                else {
                    return;
                }
            }
            else {
                return;
            }

        }
    }

    //Check Parents and sort upwards
    void SortUp(T item) {
        //To get the parent index it is (n-1)/2
        int parentIndex = (item.HeapIndex - 1) / 2;
        //While the parent index is bigger than the current
        while (true) {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0) {
                Swap(item, parentItem);
            }
            else {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    //Swap two items in an array
    void Swap(T itemA, T itemB) {
        //Swap them in the array
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        //Also need to swap heap indexes so keep temp for now
        int itemAIndex = itemA.HeapIndex;

        //Swap indexes
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

}

public interface IHeapItem<T> : IComparable<T> {
    int HeapIndex {
        get;
        set;
    }
}