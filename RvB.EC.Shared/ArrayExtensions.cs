namespace RvB.EC.Shared;

public static class ArrayExtensions {
    extension<T>(T[] array) {
        public void Deconstruct(out T item1, out T item2) {
            item1 = array[0];
            item2 = array[1];
        }

        public void Deconstruct(out T item1, out T item2, out T item3) {
            item1 = array[0];
            item2 = array[1];
            item3 = array[2];
        }

        public void Deconstruct(out T item1, out T item2, out T item3, out T item4) {
            item1 = array[0];
            item2 = array[1];
            item3 = array[2];
            item4 = array[3];
        }
    }
}
