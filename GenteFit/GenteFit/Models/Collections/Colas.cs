namespace GenteFit.Models.Collections
{
    /**
     * Para poder realizar operaciones avanzadas sobre una cola, más alla del FIFO a ciegas, hemos de crear una clase personalizada
     * usando List como base e implementar la lógica necesaria para las operaciones de Enqueue y Dequeue.
    **/
    public class Colas<T>
    {
        private List<T> Cola { get; set; }

        public Colas()
        {
            Cola = new List<T>();
        }

        public Colas(List<T> cola)
        {
            Cola = cola;
        }

        public Colas(Array cola)
        {
            // Si recibiéramos un Array, realizamos una conversión explícita.
            Cola = new List<T>((IEnumerable<T>)cola);
        }

        /* Métodos CRUD */

        public T? Enqueue(T item)
        {
            if (item != null)
            {
                try
                {
                    this.Cola.Add(item);
                    return item;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return default;
        }

        public T? Dequeue()
        {
            if (Cola.Count > 0)
            {
                try
                {
                    return Cola.First();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return default;
        }

        public T? Peek()
        {
            if (Cola.Count > 0)
            {
                try
                {
                    return Cola.ElementAt(0);
                } catch (ArgumentNullException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                } catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }  
            }

            return default;
        }

        public int IndexOf(T item)
        {
            if (item != null)
            {
                try
                {
                    return Cola.IndexOf(item);
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return -1;
        }

        public T? Update(T newItem, int position)
        {
            if (newItem != null)
            {
                try
                {
                    Cola.Insert(position, newItem);

                    return newItem;
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return default;
        }

        public int Clear()
        {
            int numElems;

            if ((numElems = Cola.Count) > 0)
            {
                try
                {
                    Cola.Clear();

                    return numElems;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return 0;
        }

        public T? Remove(T item)
        {
            if (item != null)
            {
                try
                {
                   if (Cola.Remove(item)) return item;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return default;
        }

        public bool RemoveAt(int position)
        {
            if (position >= 0)
            {
                try
                {
                    Cola.RemoveAt(position);

                    return true;
                } catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return false;
        }

        public bool Contains(T item)
        {
            if (item != null)
            {
                try
                {
                    return Cola.Contains(item);
                } catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }

            return false;
        }

        public bool IsEmpty() { return Cola.Count < 0; }

        public int Count() { return Cola.Count; }

        //TODO -> AddAll
    }
}
