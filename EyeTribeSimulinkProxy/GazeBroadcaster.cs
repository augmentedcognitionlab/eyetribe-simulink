using System;
using System.Net.Sockets;
using TETCSharpClient.Data;
using TETCSharpClient;

namespace EyeTribeSimulinkProxy
{
	public class GazeBroadcaster : IGazeListener
	{
		UdpClient client;
		double[] data = new double[8];

		public GazeBroadcaster ()
		{

		}

		public void OnGazeUpdate(GazeData gazeData)
		{
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

			try {
				client.Send (bytes, bytes.Length);
			} catch (Exception ex) {
				Console.WriteLine (ex.Message);
			}

			Console.Write (".");
		}

		public void Start(string hostname, int port) {
			client = new UdpClient (hostname, port);
			if (!GazeManager.Instance.IsActivated) {
				if (!GazeManager.Instance.Activate (GazeManager.ApiVersion.VERSION_1_0, GazeManager.ClientMode.Push))
					throw new InvalidOperationException ("Can't connect to the tracker, is Tracker Server running?");
				GazeManager.Instance.AddGazeListener(this);
			}
		}

		public void Stop() {
			try {
				client.Close ();
				if (GazeManager.Instance.IsActivated) {
					GazeManager.Instance.ClearListeners ();
					GazeManager.Instance.Deactivate ();
				}
			} catch (Exception) {
			}
		}
	}
}

