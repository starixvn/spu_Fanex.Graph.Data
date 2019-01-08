using System;

namespace GraphAPI.Service.Interface
{
    public interface IAsyncQueue<T>
    {
        void Enqueue(T data);

        void AddHandler(Action<T> handler);
    }
}