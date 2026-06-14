using System.Numerics;
using System.Timers;

namespace TTV_Calculator.Code
{
	public class BruteForceEngine
	{
		// UI accessors

		/// <summary>
		/// Sent when a new high score is found. Parameters are: bomb size, and the cold and hot tanks
		/// </summary>
		public event Action<float, GasMixtureConstructor, GasMixtureConstructor>? HighscoreUpdated;
		/// <summary>
		/// When a general status update is sent (started, paused, resumed, stopped)
		/// </summary>
		public event Action<string>? StatusChanged;
		/// <summary>
		/// Sent when computation ends, whether it be completion, cancellation, or an exception
		/// </summary>
		public event Action? Completed;

		public bool IsRunning => _running;
		public bool IsPaused => !_pauseEvent.IsSet;
		 
		// Private variables

		private CancellationTokenSource? _cts;
		private readonly ManualResetEventSlim _pauseEvent = new(true);

		private volatile bool _running;

		private BruteForceOptions _options;

		private float _globalMaxPressure;
		private readonly object _maxPressureLock = new();

		private readonly System.Timers.Timer _statusUpdateTimer = new(10_000);

		/// <summary>
		/// Releases all unmanaged resources used by the <see cref="BruteForceEngine"/> and cancels any active computation.
		/// </summary>
		public void Dispose()
		{
			_pauseEvent?.Dispose();
			_cts?.Cancel();
			_cts?.Dispose();
			_statusUpdateTimer.Stop();
			_statusUpdateTimer.Dispose();
		}

		/// <summary>
		/// Starts the brute force computation using the specified options.
		/// </summary>
		public void Start(BruteForceOptions options)
		{
			if(_running)
			{
				return;
			}

			_options = options;

			_cts = new CancellationTokenSource();
			_pauseEvent.Set();

			_running = true;
            BigInteger totalCombinations = ComputeTotalCombinations(_options);
            StatusChanged?.Invoke($"Beginning brute force with {Environment.ProcessorCount} threads. Searching {totalCombinations:N0} combinations.");
			_statusUpdateTimer.Start();

            Task.Run(() => WorkerLoop(_cts.Token, (double)totalCombinations));
        }

		/// <summary>
		/// Requests cancellation of the current brute force computation and signals all worker threads to exit.
		/// </summary>
		public void Stop()
		{
			if (!_running)
			{
				return;
			}

			_cts?.Cancel();
			_pauseEvent.Set();
			StatusChanged?.Invoke("Cancelled.");
		}

		/// <summary>
		/// Temporarily pauses the brute force computation.
		/// </summary>
		public void Pause()
		{
			if (!_running)
			{
				return;
			}

			_pauseEvent.Reset();
			_statusUpdateTimer.Stop();
			StatusChanged?.Invoke("Paused");
		}

		/// <summary>
		/// Resumes execution of a previously paused brute force computation.
		/// </summary>
		public void Resume()
		{
			if (!_running)
			{
				return;
			}

			_pauseEvent.Set();
			_statusUpdateTimer.Start();
			StatusChanged?.Invoke("Resumed");
		}

		private readonly struct BruteForceState(float cp, float ct, float hp, float ht, float[] coldGas, float[] hotGas)
		{
			public readonly float ColdPressure = cp;
			public readonly float ColdTemperature = ct;
			public readonly float HotPressure = hp;
			public readonly float HotTemperature = ht;
			public readonly float[] ColdGasPercents = coldGas;
			public readonly float[] HotGasPercents = hotGas;
		}

		/// <summary>
		/// Executes the main brute force computation loop in parallel.
		/// </summary>
		private void WorkerLoop(CancellationToken token, double totalCombinations)
		{
			long totalCompletedSimulations = 0;
			long completedSimulationsLastTenSeconds = 0;

            ElapsedEventHandler statusUpdateHandler = (_, __) =>
            {
                double cps = completedSimulationsLastTenSeconds / 10;
                completedSimulationsLastTenSeconds = 0;

                StatusChanged?.Invoke($"{cps:N0}/s {double.Round(totalCompletedSimulations / totalCombinations * 100.0)}%");
            };
            _statusUpdateTimer.Elapsed += statusUpdateHandler;

            try
			{
				Parallel.ForEach(GenerateParameterSpace(_options), new ParallelOptions
				{
					CancellationToken = token,
					MaxDegreeOfParallelism = Environment.ProcessorCount
				},
				state =>
				{
					token.ThrowIfCancellationRequested();
					_pauseEvent.Wait(token);

					RunSimulation(state);
					Interlocked.Increment(ref totalCompletedSimulations);
					Interlocked.Increment(ref completedSimulationsLastTenSeconds);
				});

				StatusChanged?.Invoke("Completed");
			}
			catch (OperationCanceledException) { }
			catch (Exception ex)
			{
				StatusChanged?.Invoke($"Error: {ex.Message}");
			}
			finally
			{
				_running = false;
                _statusUpdateTimer.Elapsed -= statusUpdateHandler;
                _statusUpdateTimer.Stop();
				Completed?.Invoke();
			}
		}

		/// <summary>
		/// Executes a gas mixture simulation for the specified brute force state.
		/// </summary>
		private void RunSimulation(BruteForceState state)
		{
			float[] coldTankContents = new float[GasLibrary.GasCount];
			float[] hotTankContents = new float[GasLibrary.GasCount];

			float maxColdTankMoles = state.ColdPressure * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * state.ColdTemperature);
			float maxHotTankMoles = state.HotPressure * 70f / (GasMixture.R_IDEAL_GAS_EQUATION * state.HotTemperature);

			for (int i = 0; i < _options.ColdGasTypes.Length; i++)
			{
				GasType gas = _options.ColdGasTypes[i];
				coldTankContents[(int)gas] = state.ColdGasPercents[i] * maxColdTankMoles;
			}

			for (int i = 0; i < _options.HotGasTypes.Length; i++)
			{
				GasType gas = _options.HotGasTypes[i];
				hotTankContents[(int)gas] = state.HotGasPercents[i] * maxHotTankMoles;
			}

			GasMixtureConstructor coldTank = new(state.ColdTemperature, 70f, coldTankContents);
			GasMixtureConstructor hotTank = new(state.HotTemperature, 70f, hotTankContents);

			GasMixture combinedTank = new();
			combinedTank.Merge(coldTank, hotTank);

			float localMaxPressure = combinedTank.Pressure;
			for (int i = 0; i < 4; i++)
			{
				ReactionType reaction = combinedTank.React();

				float pressure = combinedTank.Pressure;
				if (pressure > localMaxPressure)
				{
					localMaxPressure = pressure;
				}

				// Entropy
				if (reaction == ReactionType.None)
				{
					break;
				}
			}

			// Update global high score (thread-safe)
			lock (_maxPressureLock)
			{
				if (localMaxPressure > _globalMaxPressure)
				{
					_globalMaxPressure = localMaxPressure;
					HighscoreUpdated?.Invoke(float.Round(combinedTank.GetBombSize()), coldTank, hotTank);
				}
			}
		}

		/// <summary>
		/// Lazily generates all valid combinations of pressures, temperatures, and gas mixtures
		/// defined by the specified brute force options.
		/// </summary>
		/// <param name="opt">
		/// The brute force configuration options.
		/// </param>
		/// <returns>
		/// An enumerable sequence of <see cref="BruteForceState"/> values representing the complete
		/// parameter search space.
		/// </returns>
		/// <remarks>
		/// Values are generated on-demand using lazy iteration to avoid excessive memory usage.
		/// </remarks>
		private IEnumerable<BruteForceState> GenerateParameterSpace(BruteForceOptions opt)
		{
			IEnumerable<float[]> coldGasCombos = GenerateGasCombinations(opt.ColdGasTypes.Length, opt.ColdGasPercentStep);
			IEnumerable<float[]> hotGasCombos = GenerateGasCombinations(opt.HotGasTypes.Length, opt.HotGasPercentStep);

			// No, this isn't a good standard, but I prefer it over the billions of {} blocks
			for (float cp = opt.ColdPressureMin; cp <= opt.ColdPressureMax; cp += opt.ColdPressureStep)
			for (float ct = opt.ColdTemperatureMin; ct <= opt.ColdTemperatureMax; ct += opt.ColdTemperatureStep)
			for (float hp = opt.HotPressureMin; hp <= opt.HotPressureMax; hp += opt.HotPressureStep)
			for (float ht = opt.HotTemperatureMin; ht <= opt.HotTemperatureMax; ht += opt.HotTemperatureStep)
			foreach (var coldGas in coldGasCombos)
			foreach (var hotGas in hotGasCombos)
			{
				yield return new BruteForceState(cp, ct, hp, ht, coldGas, hotGas);
			}
		}

		/// <summary>
		/// Generates all valid gas percentage combinations that sum to 100% using the specified step size.
		/// </summary>
		/// <returns>
		/// An enumerable sequence of float arrays representing normalized gas percentage distributions.
		/// </returns>
		private static IEnumerable<float[]> GenerateGasCombinations(int gasCount, float step)
		{
			float[] buffer = new float[gasCount];

			IEnumerable<float[]> Recurse(int index, float remaining)
			{
				if (index == gasCount - 1)
				{
					buffer[index] = remaining;
					yield return buffer.ToArray();
					yield break;
				}

				for (float v = 0; v <= remaining + 1e-6f; v += step) // small epsilon to avoid float precision issues
				{
					buffer[index] = v;

					foreach (var result in Recurse(index + 1, remaining - v))
					{
						yield return result;
					}
				}
			}

			return Recurse(0, 1.0f);
		}

#region Methods for getting the total number of gas calculations
		private static BigInteger ComputeTotalCombinations(BruteForceOptions opt)
		{
			long cpCount = StepCount(opt.ColdPressureMin, opt.ColdPressureMax, opt.ColdPressureStep);
			long ctCount = StepCount(opt.ColdTemperatureMin, opt.ColdTemperatureMax, opt.ColdTemperatureStep);
			long hpCount = StepCount(opt.HotPressureMin, opt.HotPressureMax, opt.HotPressureStep);
			long htCount = StepCount(opt.HotTemperatureMin, opt.HotTemperatureMax, opt.HotTemperatureStep);

			BigInteger coldGas = GasCombinationCount(
				opt.ColdGasTypes.Length,
				opt.ColdGasPercentStep);

			BigInteger hotGas = GasCombinationCount(
				opt.HotGasTypes.Length,
				opt.HotGasPercentStep);

			return
				cpCount *
				ctCount *
				hpCount *
				htCount *
				coldGas *
				hotGas;
		}

		private static BigInteger GasCombinationCount(int gasCount, float step)
		{
			int N = (int)float.Round(1f / step);
			return Binomial(N + gasCount - 1, gasCount - 1);
		}

		private static BigInteger Binomial(int n, int k)
		{
			if (k > n - k)
			{
				k = n - k;
			}

			BigInteger result = 1;

			for (int i = 1; i <= k; i++)
			{
				result *= n - (k - i);
				result /= i;
			}

			return result;
		}

		static long StepCount(float min, float max, float step)
		{
			return (long)float.Floor((max - min) / step) + 1;
		}
#endregion
	}
}
