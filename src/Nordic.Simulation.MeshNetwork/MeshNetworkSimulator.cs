using System;
using Nordic.Abstractions.Constants;
using Nordic.Abstractions.Data;
using Nordic.Abstractions.Data.Arguments;
using Nordic.Abstractions.Enumerations;
using Nordic.Abstractions.Events;
using Nordic.Abstractions.Simulation;

namespace Nordic.Simulation.MeshNetwork
{
	[Serializable]
	public class MeshNetworkSimulator : ISimulatable
	{
		// -- fields

		private readonly MeshNetworkArgs _args;

		public event SimulatorEventHandler OnExecuting;
		public event SimulatorEventHandler Executed;

		// -- properties       

		public int Index => throw new NotImplementedException();

		public string Key => _args.Key;
		public string Name => _args.Name;

		public ArgumentsBase Arguments => _args;
		public SimulationTypes Type => SimulationTypes.Network;



		SimulationTypes ISimulatable.Type => throw new NotImplementedException();

		public bool IsActive => throw new NotImplementedException();

		ArgumentsBase ISimulatable.Arguments => throw new NotImplementedException();

		// -- constructor

		public MeshNetworkSimulator()
		{
			_args = new MeshNetworkArgs();
		}

		// -- methods

		public ISimulatable With(Action<ArgumentsBase> action)
		{
			action(_args);
			return this;
		}

		public ISimulatable OnStart()
		{
			_args.NetworkOutdated = true;
			_args.Network.SetupMatrices();
			return this;
		}

		public void Run()
		{
			OnExecuting?.Invoke(this, new SimulatorEventArgs(_args));

			if (!_args.NetworkOutdated) return;

			CalculateDistances();
			CalculateOrientations();
			_args.NetworkOutdated = false;

			Executed?.Invoke(this, new SimulatorEventArgs(_args));
		}

		// -- private method

		protected void OnStarted(object sender, SimulatorEventArgs e)
		{
			_args.Network.SetupMatrices();
		}

		/// <summary>
		/// Calculates all distances between each device.
		/// </summary>
		private void CalculateDistances()
		{
			var net = _args.Network;
			_args.Network.DistanceMatrix.Each((row, col, val) =>
			{
				var a = net[row].Position;
				var b = net[col].Position;
				return Vertex.GetLength(a, b);
			});
		}

		/// <summary>
		/// Calculates all angles (azimuth, elevation) between each device.
		/// </summary>
		private void CalculateOrientations()
		{
			var net = _args.Network;
			_args.Network.AngleMatrix.Each((r, c, v) =>
			{
				var txDev = net[r];  // Tx - transmitter
				var rxDev = net[c];  // Rx - receiver

				Vertex e_EL = new Vertex(0, 1, 0);    // Einheitsvektor Elevation
				Vertex tx_EL;                         // Sender Elevation

				Vertex e_AZ = new Vertex(0, 0, 1);    // Einheitsvektor Azimut
				Vertex tx_AZ;                         // Sender Azimut

				Vertex line = rxDev.Position - txDev.Position;

				/*
                 * ELEVATION
                 * Einheitsvektor 'e_ELEVATION' (0°) ist (0, 1, 0) -> OpenGL (+y)-Richtung
                 * Winkelberechnung zwischen den Vektoren wie sie gerendert werden
                 */
				tx_EL = Vertex.RotateRadX(Const.Math.ToRadian(txDev.Orientation.Elevation), e_EL);
				tx_EL = Vertex.RotateRadY(Const.Math.ToRadian(txDev.Orientation.Azimuth), tx_EL);
				float elevation = Const.Math.ToDegree(Vertex.ACosRad(line, tx_EL));

				/*
                 * AZIMUT
                 * Normale 'n' auf Ebene zwischen rxPos, txPos und Elevationsstab
                 * ACHTUNG der Elevationsstab vom Sender muss ausgehend von der Sendeposition gemessen werden, daher die Addition
                 */
				tx_AZ = Vertex.RotateRadX(Const.Math.ToRadian(txDev.Orientation.Elevation), e_AZ);       // erst rotatiom um Elevation-Winkel
				tx_AZ = Vertex.RotateRadY(Const.Math.ToRadian(txDev.Orientation.Azimuth), tx_AZ);        // dann Azimut-Winkel der Antenne
				Vertex n = Vertex.Normalize(rxDev.Position, txDev.Position, (txDev.Position + tx_EL));
				float azimuth = Const.Math.ToDegree(Vertex.ASinRad(n, tx_AZ));

				// Azimut-Korrekturen
				if (float.IsNaN(azimuth))   // -> is not-a-number ?
				{
					azimuth = 0;
				}
				else if (azimuth < 0)
				{
					if (Vertex.Scalar(line, tx_AZ) < 0)       // zeigen die Vektoren in die gleiche Richtung oder nicht?
					{
						azimuth = (180 - azimuth);
					}
					else
					{
						azimuth = (360 + azimuth);
					}
				}
				else if (Vertex.Scalar(line, tx_AZ) < 0)
				{
					azimuth = (180 - azimuth);
				}

				return new Angle(azimuth, elevation);
			});
		}
	}
}
