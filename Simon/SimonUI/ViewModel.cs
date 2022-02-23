using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;


namespace SimonUI
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private const int LAMP_ON_MS = 200;
        private const int LAMP_OFF_MS = 100;
        private readonly Random _random = new Random();


        private int _level = 0;
        private double _scoreMultiplier = 1;
        private List<int> _puzzle = new List<int>();
        private int _currentPositionInPuzzle = 0;
        private bool _isLevelReadyToBePlayed;
        
        private ObservableCollection<ScoreEntry> _scores = new ObservableCollection<ScoreEntry>();
        public ObservableCollection<ScoreEntry> Scores
        {
            get { return _scores; }
            set
            {
                _scores = value;
                OnPropertyChanged();
            }
        }



        private bool _isScoreInputVisible;

        public bool IsScoreInputVisible
        {
            get { return _isScoreInputVisible; }
            set
            {
                _isScoreInputVisible = value;
                OnPropertyChanged();
            }
        }

        public string PlayerName { get; set; } = "player";

        private bool _isNr1Lit;
        public bool IsNr1Lit
        {
            get { return _isNr1Lit; }
            set
            {
                _isNr1Lit = value;
                OnPropertyChanged();
            }
        }

        private bool _isNr2Lit;
        public bool IsNr2Lit
        {
            get { return _isNr2Lit; }
            set
            {
                _isNr2Lit = value;
                OnPropertyChanged();
            }
        }

        private bool _isNr3Lit;
        public bool IsNr3Lit
        {
            get { return _isNr3Lit; }
            set
            {
                _isNr3Lit = value;
                OnPropertyChanged();
            }
        }

        private bool _isNr4Lit;
        public bool IsNr4Lit
        {
            get { return _isNr4Lit; }
            set
            {
                _isNr4Lit = value;
                OnPropertyChanged();
            }
        }

        private double _score;

        public double Score
        {
            get { return _score; }
            set { _score = value; OnPropertyChanged(); }
        }



        private bool _isAnimationInProgress = false;
        public bool IsAnimationInProgress
        {
            get { return _isAnimationInProgress; }
            set
            {
                _isAnimationInProgress = value;
                OnPropertyChanged();
            }
        }


        public ICommand PlayLevelCommand { get; set; }
        public ICommand Lamp1Command { get; set; }
        public ICommand Lamp2Command { get; set; }
        public ICommand Lamp3Command { get; set; }
        public ICommand Lamp4Command { get; set; }
        public ICommand SubmitCommand { get; set; }


        public ViewModel()
        {
            CreateCommands();
            GeneratePuzzleForLevel();
            Scores = new ObservableCollection<ScoreEntry>(FileIOUtils.LoadFromJson());
        }

        private void CreateCommands()
        {
            PlayLevelCommand = new SimpleCommand(PlayLevel, () => true);
            Lamp1Command = new SimpleCommand(Lamp1Pressed, () => true);
            Lamp2Command = new SimpleCommand(Lamp2Pressed, () => true);
            Lamp3Command = new SimpleCommand(Lamp3Pressed, () => true);
            Lamp4Command = new SimpleCommand(Lamp4Pressed, () => true);
            SubmitCommand = new SimpleCommand(SubmitPressed, () => true);
        }

        private void GeneratePuzzleForLevel()
        {
            _level++;
            _puzzle.Clear();
            _currentPositionInPuzzle = 0;
            _scoreMultiplier = 1;
            for (int i = 0; i < _level; i++)
            {
                _puzzle.Add(_random.Next(1, 5));
            }
            _isLevelReadyToBePlayed = false;
        }

        private void PlayLevel()
        {
            if (_isAnimationInProgress)
            {
                return;
            }
            _scoreMultiplier *= 0.9;
            _isAnimationInProgress = true;
            var task = Task.Run(() =>
            {
                foreach (var item in _puzzle)
                {
                    SwitchLamp(item);
                }


            }).ContinueWith(t => { _isAnimationInProgress = false; _isLevelReadyToBePlayed = true; });
        }

        private void Lamp1Pressed()
        {
            if (!_isLevelReadyToBePlayed)
            {
                return;
            }
            Task.Run(() => { SwitchLamp(1); }); CheckCurrentNote(1);
        }
        private void Lamp2Pressed()
        {
            if (!_isLevelReadyToBePlayed)
            {
                return;
            }
            Task.Run(() => { SwitchLamp(2); }); CheckCurrentNote(2);
        }
        private void Lamp3Pressed()
        {
            if (!_isLevelReadyToBePlayed)
            {
                return;
            }
            Task.Run(() => { SwitchLamp(3); }); CheckCurrentNote(3);
        }
        private void Lamp4Pressed()
        {
            if (!_isLevelReadyToBePlayed)
            {
                return;
            }
            Task.Run(() => { SwitchLamp(4); }); CheckCurrentNote(4);
        }

        private void SubmitPressed()
        {
            ScoreEntry scoreEntry = new ScoreEntry(PlayerName, Score, DateTime.Now);
            _scores.Add(scoreEntry);
            Scores = new ObservableCollection<ScoreEntry>(Scores.OrderByDescending(_ => _.Score));
            FileIOUtils.SaveToJson(Scores.ToArray());
            Score = 0;
            _currentPositionInPuzzle = 0;
            GeneratePuzzleForLevel();
            IsScoreInputVisible = false;
        }

        private void CheckCurrentNote(int lamp)
        {
            if (_puzzle[_currentPositionInPuzzle] == lamp)
            {
                //ok
                _currentPositionInPuzzle++;
                if (_currentPositionInPuzzle == _puzzle.Count)
                {
                    //next level
                    _currentPositionInPuzzle = 0;
                    Score = _scoreMultiplier * Math.Exp(_level);
                    GeneratePuzzleForLevel();
                }
            }
            else
            {
                //game over
                _level = 0;
                IsScoreInputVisible = true;
            }
        }

        private void SwitchLamp(int lamp)
        {

            switch (lamp)
            {
                case 1:
                    LightLamp1();
                    break;
                case 2:
                    LightLamp2();
                    break;
                case 3:
                    LightLamp3();
                    break;
                case 4:
                    LightLamp4();
                    break;
                default:
                    throw new ArgumentException($"{nameof(SwitchLamp)} is called with invalid argument: {lamp}");


            }
        }

        private void LightLamp1()
        {
            IsNr1Lit = true;
            Thread.Sleep(LAMP_ON_MS);
            IsNr1Lit = false;
            Thread.Sleep(LAMP_OFF_MS);
        }

        private void LightLamp2()
        {
            IsNr2Lit = true;
            Thread.Sleep(LAMP_ON_MS);
            IsNr2Lit = false;
            Thread.Sleep(LAMP_OFF_MS);
        }

        private void LightLamp3()
        {
            IsNr3Lit = true;
            Thread.Sleep(LAMP_ON_MS);
            IsNr3Lit = false;
            Thread.Sleep(LAMP_OFF_MS);
        }

        private void LightLamp4()
        {
            IsNr4Lit = true;
            Thread.Sleep(LAMP_ON_MS);
            IsNr4Lit = false;
            Thread.Sleep(LAMP_OFF_MS);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
