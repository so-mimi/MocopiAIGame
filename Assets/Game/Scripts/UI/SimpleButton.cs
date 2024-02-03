using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

[RequireComponent(typeof(Button))]
public class SimpleButton : MonoBehaviour
{
    //UniRxを使ったUnit型のOnClickイベントを登録
    public Subject<Unit> OnClick = new Subject<Unit>();
    
    private void Start()
    {
        //Buttonコンポーネントを取得
        var button = GetComponent<Button>();
        //ButtonのOnClickイベントにUniRxのSubjectを登録
        button.OnClickAsObservable().Subscribe(_ => OnClick.OnNext(Unit.Default));
    }
    
}