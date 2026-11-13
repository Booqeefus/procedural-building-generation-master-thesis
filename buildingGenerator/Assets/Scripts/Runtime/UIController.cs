using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField] private DerivationTreeManager derivationTreeManager;

    private PanelRenderer panelRenderer;
    private Button oneStepButton;
    private Button fullRunButton;
    private Button resetButton;

    private void OnEnable()
    {
        panelRenderer = GetComponent<PanelRenderer>();
        if (panelRenderer != null)
        {
            // Register for the UI reload callback to receive the root visual element
            panelRenderer.RegisterUIReloadCallback(OnUIReload);
            Debug.Log("UIController: Registered for UI reload callback.");
        }
        else
        {
            Debug.LogWarning("PanelRenderer component missing on this GameObject.");
        }
    }

    private void OnDisable()
    {
        if (panelRenderer != null)
        {
            panelRenderer.UnregisterUIReloadCallback(OnUIReload);
            Debug.Log("UIController: Unregistered from UI reload callback.");
        }
    }

    private void OnUIReload(PanelRenderer renderer, VisualElement rootElement, int version)
    {
        // query the buttons and register their click events
        // BUTTON FOR ONE STEP
        oneStepButton = rootElement.Q<Button>("OneStepButton");
        if (oneStepButton != null)
        {
            oneStepButton.clicked -= OnOneStepClicked; // Prevent duplicate event subscriptions
            oneStepButton.clicked += OnOneStepClicked;
        }
        else
        {
            Debug.LogWarning("OneStepButton could not be found in the PanelRenderer asset.");
        }


        //BUTTON TO RUN REMAINDER OF RECIPE
        fullRunButton = rootElement.Q<Button>("FullRunButton");
        if (fullRunButton != null)
        {
            fullRunButton.clicked -= OnFullRunClicked; // Prevent duplicate event subscriptions
            fullRunButton.clicked += OnFullRunClicked;
        }
        else
        {
            Debug.LogWarning("FullRunButton could not be found in the PanelRenderer asset.");
        }

        //BUTTON TO RESET THE TREE
        resetButton = rootElement.Q<Button>("ResetButton");
        if (resetButton != null)
        {
            resetButton.clicked -= OnResetClicked; // Prevent duplicate event subscriptions
            resetButton.clicked += OnResetClicked;
        }
        else
        {
            Debug.LogWarning("ResetButton could not be found in the PanelRenderer asset.");
        }
    }

    private void OnOneStepClicked()
    {
        if (derivationTreeManager != null)
        {
            derivationTreeManager.PerformRecipeStep();
        }
    }

    private void OnFullRunClicked()
    {
        if (derivationTreeManager != null)
        {
            derivationTreeManager.RunWholeRecipe();
        }
    }

    private void OnResetClicked()
    {
        if (derivationTreeManager != null)
        {
            derivationTreeManager.ResetTree();
        }
    }
}