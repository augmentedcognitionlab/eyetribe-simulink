using System;
using Gtk;
using EyeTribeSimulinkProxy;

public partial class MainWindow: Gtk.Window
{
	GazeBroadcaster gb;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		gb = new GazeBroadcaster ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		gb.Stop ();
		Application.Quit ();
		a.RetVal = true;
	}

	protected void StartButtonPressed (object sender, EventArgs e)
	{
		toggleInterface ();
		gb.Start (ipEntry.Text,int.Parse(portEntry.Text));
	}

	protected void StopButtonPressed (object sender, EventArgs e)
	{
		toggleInterface ();
		gb.Stop ();
	}

	void toggleInterface() {
		startButton.Sensitive = !startButton.Sensitive;
		stopButton.Sensitive = !stopButton.Sensitive;
		ipEntry.Sensitive = !ipEntry.Sensitive;
		portEntry.Sensitive = !portEntry.Sensitive;
	}
}
