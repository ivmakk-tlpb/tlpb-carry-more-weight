// Fake IL2CPP interop types.

namespace Il2CppSystem.Collections.Generic
{
    public class HashSet<T>
    {
        private readonly System.Collections.Generic.HashSet<T> items = new System.Collections.Generic.HashSet<T>();

        public bool Add(T item) => items.Add(item);
        public bool Contains(T item) => items.Contains(item);
        public bool Remove(T item) => items.Remove(item);
    }
}

namespace Il2CppInterop.Runtime.InteropTypes
{
    public class Il2CppObjectBase
    {
        private static long next = 1;

        // Each fake object gets its own address, and a test can set one to model an address that the game reuses.
        public System.IntPtr Pointer { get; set; } = new System.IntPtr(next++);

        public T TryCast<T>() where T : Il2CppObjectBase => this as T;
    }
}
