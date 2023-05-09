# anino_exam
 
Google Drive Link for Build: https://drive.google.com/file/d/1I-xuL89NToS6aqw65R2Wcr0cbCK4b-3J/view?usp=sharing

Unity version used: Unity 2021.1.14f1

Data sources:
> Reel1-5Symbols (found in hierarchy)
       - AReel script consists of Symbol list, slot objects, and reel symbol list game object
> ASymbol script attached to each Symbol game object
       - Symbol ID (starting from zero)
       - Symbol Name ("A" = Symbol 1, "B" = Symbol 2, etc.)
       - Array of Payouts from 0 combinations to 5 combinations
> MainPlayer script attached to "MainPlayer" game object
       - Starting coins of player can be edited in-script at Start function

Possible future improvements:
> Need to implement randomization of symbols per reel
> Need to fix how the slotResult 2D array recognizes the resulting slot machine spin result after spinning, as offsets to real result is seen
> Add UI Sounds and Sound Effects for spinning, winning, and adding/decreasing bets

