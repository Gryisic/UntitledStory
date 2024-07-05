using System;
using System.Collections.Generic;
using System.Threading;
using Common.Dialogues.Utils;
using Cysharp.Threading.Tasks;
using Infrastructure.Utils;
using Ink.Runtime;

namespace Common.Dialogues
{
    public class Dialogue
    {
        private readonly InkStoryParser _inkStoryParser;
        private readonly InkFunctionsResolver _functionsResolver;
        
        private bool _canTypeNextSentence = true;
        private bool _canTypeSentenceAsync = true;
        
        private bool _isSentenceFinished;
        private bool _isInAutoMode;

        private int _choiceIndex;
        private bool _isChoiceTaken;

        private SpeakerData _lastSpeakerData;

        private Story _story;

        public event Action<IReadOnlyList<Choice>> ChoicesSelectionRequested;
        public event Action<SpeakerData> SpeakerDataUpdated;
        public event Action<SpeakerData> MonologueEnded; 
        public event Action<string> LetterPrinted;
        public event Action<bool> SentencePrinting;
        public event Action Ended;

        public Dialogue(InkFunctionsResolver functionsResolver)
        {
            _functionsResolver = functionsResolver;
            
            _inkStoryParser = new InkStoryParser();
        }
        
        public void Start(CancellationToken token)
        {
            _functionsResolver.Bind(_story);

            TypeMonologueAsync(token).Forget();
        }

        public void SetStory(Story story) => _story = story;

        public void ToggleAutoMode()
        {
            if (_isInAutoMode == false)
            {
                _isInAutoMode = true;

                if (_isSentenceFinished)
                    _canTypeNextSentence = true;
            }
            else
            {
                _isInAutoMode = false;
            }
        }

        public void NextSentence() 
        {
            if (_isSentenceFinished == false)
                _canTypeSentenceAsync = false;
            else
                _canTypeNextSentence = true;
        }

        public void SetChoice(int index)
        {
            _choiceIndex = index;
            _isChoiceTaken = true;
        }

        private async UniTask TypeMonologueAsync(CancellationToken token) 
        {
            while (token.IsCancellationRequested == false && _story.canContinue)
            {
                _canTypeNextSentence = false;
                
                UniTask typeSentenceTask = TypeSentenceAsync(_story.Continue(), token);
                UniTask awaitPermissionToType = UniTask.WaitUntil(() => _canTypeNextSentence, cancellationToken: token);

                await UniTask.WhenAll(typeSentenceTask, awaitPermissionToType);

                if (_story.currentChoices.Count > 0)
                {
                    _isChoiceTaken = false;
                    
                    await MakeChoiceAsync(token);
                }
                
                MonologueEnded?.Invoke(_lastSpeakerData);
            }

            _functionsResolver.UnBind(_story);
            
            Ended?.Invoke();
        }

        private async UniTask TypeSentenceAsync(string sentence, CancellationToken token)
        {
            sentence = sentence.Trim();

            if (sentence == string.Empty)
            {
                _canTypeNextSentence = true;
                
                return;
            }
            
            _inkStoryParser.Parse(sentence, _story.currentTags, out InkParsedText parsedText);
            
            string typedSentence = "";
            string speaker;

            if (string.IsNullOrEmpty(parsedText.SpeakerData.Speaker) == false)
            {
                speaker = parsedText.SpeakerData.Speaker;

                _lastSpeakerData = parsedText.SpeakerData;
            }
            else
            {
                speaker = _lastSpeakerData.Speaker;
            }
            
            if (string.IsNullOrEmpty(speaker))
                throw new Exception($"Speaker of sentence {sentence} is unknown");
            
            _canTypeSentenceAsync = true;
            _isSentenceFinished = false;

            SentencePrinting?.Invoke(true);
            
            if (parsedText.SpeakerData.Speaker != null)
                SpeakerDataUpdated?.Invoke(parsedText.SpeakerData);
            
            sentence = parsedText.Sentence;

            char[] letters = sentence.ToCharArray();
            
            for (var i = 0; i < letters.Length; i++)
            {
                char letter = letters[i];
                
                if (parsedText.HasHiddenParts && i == parsedText.NextHiddenPartIndex)
                {
                    for (int j = i; j < parsedText.NextVisiblePartIndex; j++)
                    {
                        letter = letters[j];
                        typedSentence += letter;
                    }
                    
                    i = parsedText.NextVisiblePartIndex - 1;
                    parsedText.ToNextHiddenPart();
                    
                    continue;
                }
                
                if (_canTypeSentenceAsync == false)
                {
                    typedSentence = sentence;

                    LetterPrinted?.Invoke(typedSentence);

                    break;
                }

                typedSentence += letter;

                await UniTask.Delay(TimeSpan.FromSeconds(Constants.DialogueLetterPrintTime), cancellationToken: token);

                LetterPrinted?.Invoke(typedSentence);
            }

            SentencePrinting?.Invoke(false);
            _isSentenceFinished = true;

            if (_isInAutoMode)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(Constants.AutoDialogueModeAwaitTime), cancellationToken: token);

                _canTypeNextSentence = true;
            }
        }

        private async UniTask MakeChoiceAsync(CancellationToken token)
        {
            ChoicesSelectionRequested?.Invoke(_story.currentChoices);

            await UniTask.WaitUntil(() => _isChoiceTaken, cancellationToken: token);
            
            _story.ChooseChoiceIndex(_choiceIndex);
        }
    }
}