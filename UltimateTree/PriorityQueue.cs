using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeAndHashtable
{
    /*By-the-book PriorityQueue*/
    public class PriorityQueue
    {
        public struct HeapEntry
        {
            private object item;
            private long priority;
            public HeapEntry(object item, long priority)
            {
                this.item = item;
                this.priority = priority;
            }
            public object Item
            {
                get
                {
                    return item;
                }
            }
            public long Priority
            {
                get
                {
                    return priority;
                }
            }
        }

        private long count;
        private long capacity;
        private bool descending; //Means that the max element at the top
        private HeapEntry[] heap;

        public long Count
        {
            get
            {
                return this.count;
            }
        }

        public PriorityQueue(long capacity, bool descending)
        {
            this.capacity = capacity;
            this.descending = descending;
            heap = new HeapEntry[this.capacity];
        }

        public object Dequeue()
        {
            object result = heap[0].Item;
            count--;
            trickleDown(0, heap[count]);
            return result;
        }

        public void Enqueue(object item, long priority)
        {
            count++;
            bubbleUp(count - 1, new HeapEntry(item, priority));
        }

        private void bubbleUp(long index, HeapEntry he)
        {
            long parent = (index - 1) / 2;
            // note: (index > 0) means there is a parent
            while (
                   (index > 0) &&
                   ((this.descending && heap[parent].Priority < he.Priority) || (!this.descending && heap[parent].Priority > he.Priority))
                  )
            {
                heap[index] = heap[parent];
                index = parent;
                parent = (index - 1) / 2;
            }
            heap[index] = he;
        }

        private void trickleDown(long index, HeapEntry he)
        {
            long child = (index * 2) + 1;
            while (child < count)
            {
                if (
                    ((child + 1) < count) &&
                    ((this.descending && heap[child].Priority < heap[child + 1].Priority) || (!this.descending && heap[child].Priority > heap[child + 1].Priority))
                   )
                {
                    child++;
                }
                heap[index] = heap[child];
                index = child;
                child = (index * 2) + 1;
            }
            bubbleUp(index, he);
        }
    }
}
