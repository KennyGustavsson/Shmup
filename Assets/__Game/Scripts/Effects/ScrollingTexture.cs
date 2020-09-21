using UnityEngine;

public class ScrollingTexture : MonoBehaviour{
	public enum ScrollType{
		Vertical,
		Horizontal,
		Both
	}
	public ScrollType type;
	public float scrollSpeed = 0.1f;
	
	private Mesh _mesh;
	private bool _inMenu;

	private void Awake(){
		_mesh = GetComponent<MeshFilter>().mesh;
		if (!GameManager.Instance) _inMenu = true;
	}

	private void FixedUpdate(){
		UVScroll();
	}

	private void UVScroll(){
		var uvScroll = _mesh.uv;
		
		switch ((int)type){
			case 0:
				for (int i = 0; i < uvScroll.Length; i++){
					if (!_inMenu){
						uvScroll[i] += new Vector2(0, scrollSpeed * Time.deltaTime * GameManager.Instance.speedMultiplier);
					}
					else{
						uvScroll[i] += new Vector2(0, scrollSpeed * Time.deltaTime);
					}
				}
				break;
			case 1:
				for (int i = 0; i < uvScroll.Length; i++){
					if (!_inMenu){
						uvScroll[i] += new Vector2(scrollSpeed * Time.deltaTime * GameManager.Instance.speedMultiplier, 0);
					}
					else{
						uvScroll[i] += new Vector2(scrollSpeed * Time.deltaTime, 0);
					}
				}
				break;
			case 2:
				for (int i = 0; i < uvScroll.Length; i++){
					if (!_inMenu){
						uvScroll[i] += new Vector2(scrollSpeed * Time.deltaTime * GameManager.Instance.speedMultiplier,
							scrollSpeed * Time.deltaTime * GameManager.Instance.speedMultiplier);
					}
					else{
						uvScroll[i] += new Vector2(scrollSpeed * Time.deltaTime,
							scrollSpeed * Time.deltaTime);
					}
				}
				break;
		}
		
		_mesh.uv = uvScroll;
	}
}