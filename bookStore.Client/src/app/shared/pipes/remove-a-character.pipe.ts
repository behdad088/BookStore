import {Pipe} from "angular2/core";
 
@Pipe({
	name : "removeCharacterT"
})
 
export class RemoveCharacterT{
	transform(value){
		if(value != null){
			return value.replace(/T/g, " ");
		}
	}
}