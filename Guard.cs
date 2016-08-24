using System;
using SubrealTeam.Windows.Common.Extensions;

namespace SubrealTeam.Windows.Common
{
	/// <summary>
	/// Реализует методы по защите параметров и локальных переменных.
	/// </summary>
	public static class Guard
	{
		/// <summary>
		/// Вызывает исключение типа <typeparamref name="TException"/> с указанным сообщением
		/// когда утверждение верно.
		/// </summary>
		/// <typeparam name="TException">Тип вызываемого исключения.</typeparam>
		/// <param name="assertion"> Условие для проверки. Если true, тогда вызывается исключение <typeparamref name="TException"/>.</param>
		/// <param name="message"> Строка сообщения для вызываемого исключения.</param>
		public static void Against<TException>(bool assertion, string message) where TException : Exception
		{
			if (assertion)
				throw (TException) Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// Вызывает исключение типа <typeparamref name="TException"/> с указанным сообщением
		/// когда утверждение верно.
		/// </summary>
		/// <typeparam name="TException"> Тип вызываемого исключения.</typeparam>
		/// <param name="assertion">Условие для проверки. Если true, тогда вызывается исключение <typeparamref name="TException"/>.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void Against<TException>(Func<bool> assertion, string message) where TException : Exception
		{
			//Execute the lambda and if it evaluates to true then throw the exception.
			if (assertion())
				throw (TException) Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// Вызывает <see cref="InvalidOperationException"/> когда указанный экземпляр
		/// не унаследован напрямую от типа <typeparamref name="TBase"/>.
		/// </summary>
		/// <typeparam name="TBase">Базовый тип для проверки</typeparam>
		/// <param name="instance">Объект для которого проверяется наследование от типа <typeparamref name="TBase"/>.</param>
		/// <param name="paramName">Наименование экемпляра</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void InheritsFrom<TBase>(object instance, string paramName = "",
			string message = "Экземпляр {0} не унаследован напрямую от типа {1}") // where TBase : Type
		{
			InheritsFrom<TBase>(instance.GetType(), message.Args(paramName, typeof(TBase)));
		}

		/// <summary>
		/// Вызывает <see cref="InvalidOperationException"/> когда указанный тип
		/// не унаследован напрямую от типа <typeparamref name="TBase"/>.
		/// </summary>
		/// <typeparam name="TBase">Базовый тип для проверки.</typeparam>
		/// <param name="type">Тип <see cref="Type"/> для проверки унаследованости от типа <typeparamref name="TBase"/>.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void InheritsFrom<TBase>(Type type, string message = "")
		{
			if (type.BaseType != typeof(TBase))
				throw new InvalidOperationException(
					message.NotEmpty("Тип {0} не наследован напрямую от типа {1}".Args(type, typeof(TBase))));
		}

		/// <summary>
		/// Вызывает <see cref="InvalidOperationException"/> когда указанный экземпляр
		/// не унаследован от типа <typeparamref name="TBase"/>.
		/// </summary>
		/// <typeparam name="TBase">Базовый тип для проверки</typeparam>
		/// <param name="instance">Объект для которого проверяется наследование от типа <typeparamref name="TBase"/>.</param>
		/// <param name="paramName">Наименование экемпляра</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void IsSubclassOf<TBase>(object instance, string paramName = "",
			string message = "Экземпляр {0} не унаследован от типа {1}")
		{
			if (!instance.GetType().IsSubclassOf(typeof(TBase)))
				throw new InvalidOperationException(message.Args(paramName, typeof(TBase)));
		}

		/// <summary>
		/// Вызывает <see cref="InvalidOperationException"/> когда указанный экземпляр
		/// не реализует интерфейс <typeparamref name="TInterface"/>.
		/// </summary>
		/// <typeparam name="TInterface">Интерфейс который должен реализовывать эеземпляр.</typeparam>
		/// <param name="instance">Экземпляр, для которого проверяется реазизация интерфейса <typeparamref name="TInterface"/>.</param>
		/// <param name="paramName">Наименование экемпляра</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void Implements<TInterface>(object instance, string paramName = "",
			string message = "Экземпляр {0} не реализует интерфейс {1}")
		{
			Implements<TInterface>(instance.GetType(), message.Args(paramName, typeof(TInterface)));
		}

		/// <summary>
		/// Вызывает <see cref="InvalidOperationException"/> когда указанный тип
		/// не реализует интерфейс <typeparamref name="TInterface"/>.
		/// </summary>
		/// <typeparam name="TInterface">Тип интерфейса <paramref name="type"/> который должен быть реализован.</typeparam>
		/// <param name="type">Тип <see cref="Type"/>, для которого проверяется реализация интерфейса <typeparamref name="TInterface"/>.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void Implements<TInterface>(Type type, string message = "")
		{
			if (!typeof(TInterface).IsAssignableFrom(type))
				throw new InvalidOperationException(
					message.NotEmpty("Тип {0} не реализует интерфейс {1}".Args(type, typeof(TInterface))));
		}

		/// <summary>
		/// Вызывает <see cref="InvalidOperationException"/> когда указанный экземпляр
		/// не указанного типа.
		/// </summary>
		/// <typeparam name="TType">Тип на соотвествие которому проверяется <paramref name="instance"/>.</typeparam>
		/// <param name="instance">Экземпляр, для которого выполняется проверка.</param>
		/// <param name="paramName">Наименование экемпляра</param>
		/// <param name="message">Сообщение для исключения <see cref="InvalidOperationException"/>.</param>
		public static void TypeOf<TType>(object instance, string paramName = "",
			string message = "Экземпляр {0} не является типом {1}")
		{
			if (!(instance is TType))
				throw new InvalidOperationException(message.Args(paramName, typeof(TType)));
		}

		/// <summary>
		/// Вызывает исключение, когда экземпляр объекта не равен другому экземпляру объекта.
		/// </summary>
		/// <typeparam name="TException">Тип вызываемого исключения.</typeparam>
		/// <param name="compare">Сравниваемый экземпляр объекта </param>
		/// <param name="instance">Экземпляр объекта, с которым сравнивается.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void IsEqual<TException>(object compare, object instance, string message) where TException : Exception
		{
			if (compare != instance)
				throw (TException) Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// Вызывает исключение Exception, когда экземпляр объекта не равен другому экземпляру объекта.
		/// </summary>
		/// <param name="compare">Сравниваемый экземпляр объекта </param>
		/// <param name="instance">Экземпляр объекта, с которым сравнивается.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void IsEqual(object compare, object instance, string message)
		{
			IsEqual<Exception>(compare, instance, message);
		}

		/// <summary>
		/// Вызывает исключение, когда экземпляр объекта является значением null.
		/// </summary>
		/// <param name="instance">Экземпляр объекта.</param>
		/// <param name="paramName">Имя параметра, вызвавшего данное исключение.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void IsNotNull(object instance, string paramName = "",
			string message = "Экземпляр является значением null")
		{
			if (ReferenceEquals(instance, null))
				throw new ArgumentNullException(paramName, message);
		}

		/// <summary>
		/// Вызывает исключение, когда заданная строка является значением null, пустой строкой или строкой, состоящей только из пробельных символов.
		/// </summary>
		/// <param name="value">Заданная строка.</param>
		/// <param name="paramName">Имя параметра, вызвавшего данное исключение.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void IsNotEmpty(string value, string paramName = "", string message = "Строка не заполнена")
		{
			if (value.IsEmpty())
				throw new ArgumentNullException(paramName, message);
		}

		/// <summary>
		/// Вызывает исключение, когда заданный идентификатор является значением null или Guid.Empty
		/// </summary>
		/// <param name="value">Заданный идентификатор.</param>
		/// <param name="paramName">Имя параметра, вызвавшего данное исключение.</param>
		/// <param name="message">Строка сообщения для вызываемого исключения.</param>
		public static void IsNotEmpty(Guid value, string paramName = "", string message = "Пустой идентификатор")
		{
			if (value == null || value == Guid.Empty)
				throw new ArgumentNullException(paramName, message);
		}
	}
}
