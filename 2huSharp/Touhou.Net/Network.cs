using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Net;
using System.Net.Sockets;

namespace Touhou.Net {
	public class Network {

		public void Host(string port) {
			_listener = new TcpListener(IPAddress.Any, int.Parse(port));
			
			_listener.Start();
			_client = _listener.AcceptTcpClient();
			
			_stream = _client.GetStream();
		}

		public Result Connect(string address, string port) {
			_client = new TcpClient();

			

			try {
				_client.Connect(IPAddress.Parse(address), int.Parse(port));
			} catch {
				return Result.Failure;
			}

			return Result.Success;
			
		}

		private TcpListener _listener;
		private TcpClient _client;
		private NetworkStream _stream;

		public enum Result { Success, Failure }
	}
}
