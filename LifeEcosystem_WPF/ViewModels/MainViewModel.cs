using LifeEcosystem_WPF.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public string ZoomButtonText => $"Przełącz Zoom ({ZoomLevel}x)";
        private double zoomLevel = 1.0;
        public double ZoomLevel
        {
            get => zoomLevel;
            set
            {
                zoomLevel = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ZoomButtonText));
            }
        }



        public ICommand ToggleZoomCommand { get; }

        //komendy dla przycisków
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand StepCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand RandomizeCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }


        public MainViewModel()
        {

            board = new Board(50, 50);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(intervalMs);
            timer.Tick += (s, e) => ExecuteStep();
            //przyciski komend
            ToggleZoomCommand = new RelayCommand(_ => ZoomLevel = ZoomLevel == 1.0 ? 2.5 : 1.0);
            StartCommand = new RelayCommand(_ => Start(), _ => !isRunning);
            StopCommand = new RelayCommand(_ => Stop(), _ => isRunning);
            StepCommand = new RelayCommand(_ => ExecuteStep(), _ => !isRunning);
            ClearCommand = new RelayCommand(_ => Clear(), _ => !isRunning);
            RandomizeCommand = new RelayCommand(_ => Randomize(), _ => !isRunning);
            SaveCommand = new RelayCommand(_ => SaveToFile(), _ => !isRunning);
            LoadCommand = new RelayCommand(_ => LoadFromFile(), _ => !isRunning);
        }


        private void Start()
        {
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
