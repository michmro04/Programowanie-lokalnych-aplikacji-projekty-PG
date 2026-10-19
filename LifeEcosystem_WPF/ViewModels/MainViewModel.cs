using LifeEcosystem_WPF.Models;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Threading;

namespace LifeEcosystem_WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Board board;
        public Board Board
        {
            get => board;
            set
            {
                board = value;
                OnPropertyChanged();
            }

        }
        private DispatcherTimer timer;



        private bool isRunning;
        public bool CanEdit => !isRunning;
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                isRunning = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanEdit));
            }
        }

        
        private int intervalMs = 200;
        public int IntervalMs
        {
            get => intervalMs;
            set
            {
                intervalMs = value;

                if (timer != null)
                {
                    timer.Interval = TimeSpan.FromMilliseconds(intervalMs);
                }
                    OnPropertyChanged();
            }
        }

        public string ZoomButtonText => $"Przełącz Zoom ({ZoomLevel:0.0}x)";
        private double zoomLevel = 1.0;
        public double ZoomLevel
        {
            get => zoomLevel;
            set
            {
                zoomLevel = Math.Round(Math.Clamp(value,0.5,4.0),2);
                OnPropertyChanged();
                OnPropertyChanged(nameof(ZoomButtonText));
            }
        }


        private string plantColor = "#4CAF50";
        public string PlantColor
        {
            get => plantColor;
            set
            {
                plantColor = value;
                OnPropertyChanged();
            }
        }


        private string animalColor = "#E53935";
        public string AnimalColor
        {
            get => animalColor;
            set
            {
                animalColor = value;
                OnPropertyChanged();
            }
        }

        private string displayStyle = "Kółko";
        public string DisplayStyle
        {
            get => displayStyle;
            set
            {
                displayStyle = value;
                OnPropertyChanged();
            }
        }

        //komendy dla przycisków
        public ICommand ZoomInCommand { get; }
        public ICommand ZoomOutCommand { get; }


        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand StepCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand RandomizeCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }


        public MainViewModel(int width = 50, int height = 50)
        {

            board = new Board(width, height);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(intervalMs);
            timer.Tick += (s, e) => ExecuteStep();
            //przyciski komend
            ZoomInCommand = new RelayCommand(_ => ZoomLevel += 0.5);
            ZoomOutCommand = new RelayCommand(_ => ZoomLevel -= 0.5);
            StartCommand = new RelayCommand(_ => Start(), _ => !isRunning);
            StopCommand = new RelayCommand(_ => Stop(), _ => isRunning);
            StepCommand = new RelayCommand(_ => ExecuteStep(), _ => !isRunning);
            ClearCommand = new RelayCommand(_ => Clear(), _ => !isRunning);
            RandomizeCommand = new RelayCommand(_ => Randomize(), _ => !isRunning);
            SaveCommand = new RelayCommand(_ => SaveToFile(), _ => !isRunning);
            LoadCommand = new RelayCommand(_ => LoadFromFile(), _ => !isRunning);
        }

        private bool startFlag = false;
        private void Start()
        {
            if(board.Stats.TotalPlants==0 && board.Stats.TotalAnimals==0)
            {
                board.Randomize(0.1, 0.2);
                OnPropertyChanged(nameof(Board));
            }
                timer.Start();
                IsRunning = true;
        }

        private void Stop()
        {
            timer.Stop();
            IsRunning = false;
        }

        private void ExecuteStep()
        {
            board.Step();
            OnPropertyChanged(nameof(Board));
        } 

        private void Clear()
        {
            board.Clear();
            OnPropertyChanged(nameof(Board));
        }

        private void Randomize()
        {
            board.Randomize(0.1, 0.2);
            OnPropertyChanged(nameof(Board));
        }

        private void SaveToFile()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Plik tekstowy (*.txt)|*.txt",
                FileName = "life_ecosystem_wpf.txt"
            };

            if (sfd.ShowDialog() == true)
            {
                Board.SaveToFile(sfd.FileName);
            }
        }

        private void LoadFromFile()
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "Plik tekstowy (*.txt)|*.txt"
            };

            if (ofd.ShowDialog() == true)
            {
                Board = Board.LoadFromFile(ofd.FileName);
            }
        }




    }
}
