namespace TU.Sharp
{
    public delegate void RefAction<T1>(ref T1 arg1);
    public delegate void RefAction<T1, T2>(ref T1 arg1, T2 arg2);

}