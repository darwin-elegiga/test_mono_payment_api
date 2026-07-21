package templates

#InitContainer: {
	name:            string
	image:           string
	imagePullPolicy: string
	command?: [...string]
	resources?: {
		requests?: {
			cpu?:    string
			memory?: string
		}
	}
	terminationMessagePath?: string
	terminationMessagePolicy?: string
	volumeMounts?: [...{
		name: string
		mountPath: string
		readOnly?: bool
	}]
}