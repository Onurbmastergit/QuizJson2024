using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonAlternativa : MonoBehaviour
{
    public int index = 0;
    public TextMeshProUGUI texto;
    public bool isCase = true;

    public void SetForCaso()
    {
        var sistemaCaso = CaseManager.Instance.GetComponent<SistemaCasos>();
        if (sistemaCaso == null) return;

        sistemaCaso.ClicarBotaoAlternativa(index);
    }

    public void SetForPerguntas()
    {
        var sistemaPergunta = CaseManager.Instance.GetComponent<SistemaPerguntas>();
        if( sistemaPergunta == null) return;

        sistemaPergunta.ClicarBotaoAlternativa(index);
    }

    public void Select()
    {
        if(isCase) SetForCaso();
        else SetForPerguntas();
    }

    public static ButtonAlternativa Spawn(Transform parent, int index, string text, bool setForCaso)
    {
        var resourceButton = Resources.Load<ButtonAlternativa>("Prefab/Alternativa");

        var instance = Instantiate(resourceButton, parent);

        instance.index = index;

        instance.texto.text = text;

        instance.isCase = setForCaso;

        return instance;
    }
}
