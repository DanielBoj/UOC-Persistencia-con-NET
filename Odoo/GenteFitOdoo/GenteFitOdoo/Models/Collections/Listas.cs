namespace GenteFit.Models.Collections
{
    public class Listas<T>
    {
        private List<T> Lista { get; set; }

        public Listas()
        {
            Lista = new List<T>();
        }

        public Listas(List<T> lista)
        {
            Lista = lista;
        }

        public Listas(Array lista)
        {
            // Si recibiéramos un Array, realizamos una conversión explícita.
            Lista = new List<T>((IEnumerable<T>)lista);
        }

        virtual public T? Add(T item)
        {
            if (item == null)
            {
                return default;
            }

            try
            {
                Lista.Add(item);

                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return default;
            }
        }

        public T? Remove(T item)
        {
            if (item != null && Lista.Count > 0)
            {
                try
                {
                    if (Lista.Remove(item)) return item;
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
            return default;
        }

        public bool RemoveAt(int position)
        {
            if ( position >= 0 && Lista.Count > 0)
            {
                try
                {
                    Lista.RemoveAt(position);

                    return true;
                } catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return false;
        }

        public T? Get(int position)
        {
            if (position >= 0 && Lista.Count > 0)
            {
                try
                {
                    return Lista[position];
                } catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
                {

                }
            }
            return default;
        }

        public T? Update(T newItem, int position)
        {
            if (newItem == null)
            {
                return default;
            }

            try
            {
                Lista.Insert(position, newItem);

                return newItem;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return default;
            }
        }

        public int Clear()
        {
            int numElems;

            if ((numElems = Lista.Count) > 0)
            {
                try
                {
                    Lista.Clear();

                    return numElems;
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }

            }

            return 0;
        }

        public int IndexOf(T item)
        {
            if (item != null && Lista.Count > 0)
            {
                try
                {
                    return Lista.IndexOf(item);
                } catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return -1;
        }

        public bool Contains(T item)
        {
            if (item != null && Lista.Count > 0)
            {
                try
                {
                    return Lista.Contains(item);
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return false;
        }

        public int Count() { return Lista.Count; }

        public bool IsEmpty() { return Lista.Count < 0; }
    }
}
