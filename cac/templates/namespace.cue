package templates

#Namespace: {
	apiVersion: "v1"
	kind:       "Namespace"
	metadata: {
		name: string
		labels?: {
			[string]: string
		}
	}
}
