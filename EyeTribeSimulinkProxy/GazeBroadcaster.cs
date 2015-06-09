using System;
using System.Net.Sockets;
using TETCSharpClient;
using TETCSharpClient.Data;

namespace EyeTribeSimulinkProxy
{
	public class GazeBroadcaster : IGazeListener
	{
		UdpClient client;
		double[] data = new double[8];

		public GazeBroadcaster ()
		{
			// Register this class for events
			GazeManager.Instance.AddGazeListener(this);
		}

		public void OnGazeUpdate(GazeData gazeData)
		{
			double gX = gazeData.SmoothedCoordinates.X;
			double gY = gazeData.SmoothedCoordinates.Y;

			data [0] = gazeData.TimeStamp;
			data [1] = gazeData.IsFixated ? 1 : 0;
			data [2] = gazeData.SmoothedCoordinates.X;
			data [3] = gazeData.SmoothedCoordinates.Y;
			data [4] = gazeData.RawCoordinates.X;
			data [5] = gazeData.RawCoordinates.Y;
			data [6] = gazeData.LeftEye.PupilSize;
			data [7] = gazeData.RightEye.PupilSize;

			byte[] bytes = new byte[data.Length*sizeof(double)];
			Buffer.BlockCopy (data, 0, bytes, 0, bytes.Length);
			client.Send (bytes, bytes.Length);
		}

		public void Start(string hostname, int port) {
			client = new UdpClient (hostname, port);
			if (!GazeManager.Instance.IsActivated)
				GazeManager.Instance.Activate(GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push);
		}

		public void Stop() {
			client.Close ();
			if (GazeManager.Instance.IsActivated)
				GazeManager.Instance.Deactivate ();
		}
	}
}

