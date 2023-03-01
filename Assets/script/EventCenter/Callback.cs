//这里的CallBack其实是一个delegate类型的变量
public delegate void Callback();
public delegate void Callback<T>(T arg);
public delegate void Callback<T, X>(T arg1,X arg2);
public delegate void Callback<T, X, Y>(T arg1, X arg2, Y arg3);
public delegate void Callback<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);